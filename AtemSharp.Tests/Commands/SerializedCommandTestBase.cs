using AtemSharp.Commands;
using AtemSharp.Tests.TestUtilities.CommandTests;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace AtemSharp.Tests.Commands;

public abstract class SerializedCommandTestBase<TCommand, TTestData>
    where TCommand : SerializedCommand
    where TTestData : SerializedCommandTestBase<TCommand, TTestData>.CommandDataBase, new()
{
    /// <summary>
    /// Override to specify which byte ranges contain floating-point encoded data
    /// that should be compared with tolerance for precision differences
    /// </summary>
    protected virtual Range[] GetFloatingPointByteRanges()
    {
        return [];
    }

    private bool IsFloatingPointByte(int index, int totalLength)
    {
        var ranges = GetFloatingPointByteRanges();
        foreach (var range in ranges)
        {
            var (start, length) = range.GetOffsetAndLength(totalLength);
            var end = start + length - 1;
            if (index >= start && index <= end)
            {
                return true;
            }
        }

        return false;
    }

    private bool AreApproximatelyEqual(byte[] actual, byte[] expected)
    {
        if (actual.Length != expected.Length)
        {
            return false;
        }

        for (var i = 0; i < actual.Length; i++)
        {
            var tolerance = IsFloatingPointByte(i, actual.Length) ? 2 : 0;
            if (Math.Abs(actual[i] - expected[i]) > tolerance)
            {
                // Check if its within the tolerance with carry
                return !(Math.Abs(actual[i] - expected[i]) < 256 - tolerance);
            }
        }

        return true;
    }

    [UsedImplicitly(ImplicitUseTargetFlags.WithInheritors | ImplicitUseTargetFlags.WithMembers)]
    public abstract class CommandDataBase : TestUtilities.CommandTests.CommandDataBase
    {
        public uint Mask { get; set; }
    }

    public static IEnumerable<TestCaseData> GetTestCases()
    {
        var testCases = Helper.GetTestCases<TCommand, TTestData>().ToArray();
        Assert.That(testCases.Length, Is.GreaterThan(0), "No test cases found");
        return testCases;
    }

    public static TestCaseData CreateMergingTestCase<TValue>(
                                                      string propertyName,
                                                      TValue firstValue,
                                                      TValue secondValue)
    {
        return new TestCaseData(propertyName, firstValue, secondValue).SetName(propertyName);
    }

    public void TestPropertyMerging<TValue>(
        Func<TCommand> factory,
        string propertyName,
        TValue firstValue,
        TValue secondValue)
    {
        var first = factory();
        var second = factory();

        var getter = typeof(TCommand).GetProperty(propertyName)?.GetMethod ?? throw new  InvalidOperationException($"No getter for property {typeof(TCommand)}.{propertyName} found");
        var setter = typeof(TCommand).GetProperty(propertyName)?.SetMethod ?? throw new  InvalidOperationException($"No setter for property {typeof(TCommand)}.{propertyName} found");

        setter.Invoke(first, [firstValue]);
        setter.Invoke(second, [secondValue]);

        Assert.That(second.TryMergeTo(first), Is.True);
        Assert.That(getter.Invoke(first, []), Is.EqualTo(secondValue));
    }

    public void TestPropertyNonMerging<TValue>(
        Func<TCommand> factory,
        string propertyName,
        TValue firstValue,
        TValue secondValue)
    {
        var first = factory();
        var second = factory();

        var getter = typeof(TCommand).GetProperty(propertyName)?.GetMethod ?? throw new  InvalidOperationException($"No getter for property {typeof(TCommand)}.{propertyName} found");
        var setter = typeof(TCommand).GetProperty(propertyName)?.SetMethod ?? throw new  InvalidOperationException($"No setter for property {typeof(TCommand)}.{propertyName} found");

        setter.Invoke(first, [firstValue]);

        Assert.That(second.TryMergeTo(first), Is.True);
        Assert.That(getter.Invoke(first, []), Is.EqualTo(firstValue));
    }

    [Test, TestCaseSource(nameof(GetTestCases))]
    public void TestSerialization(TestUtilities.CommandTests.TestCaseData<TTestData> testCase)
    {
        if (testCase.Command.UnknownProperties.Count != 0)
        {
            Assert.Fail("Unprocessed test data:\n" + JsonConvert.SerializeObject(testCase.Command.UnknownProperties));
        }

        var expectedPayload = testCase.Payload;

        var command = CreateSut(testCase);
        command.Flag = testCase.Command.Mask;

        // Act
        var actualPayload = command.Serialize(testCase.FirstVersion);

        Assert.That(actualPayload, Has.Length.EqualTo(expectedPayload.Length));

        // Step 1: Compare non-float bytes exactly
        var actualNonFloatBytes =
            string.Join("-", actualPayload.Select((b, i) => IsFloatingPointByte(i, actualPayload.Length) ? "XX" : $"{b:X2}"));
        var expectedNonFloatBytes =
            string.Join("-", expectedPayload.Select((b, i) => IsFloatingPointByte(i, actualPayload.Length) ? "XX" : $"{b:X2}"));
        Assert.That(actualNonFloatBytes, Is.EqualTo(expectedNonFloatBytes));

        // Then try approximate match for floating-point fields
        var actualFloatBytes =
            string.Join("-", actualPayload.Select((b, i) => !IsFloatingPointByte(i, actualPayload.Length) ? "XX" : $"{b:X2}"));
        var expectedFloatBytes =
            string.Join("-", expectedPayload.Select((b, i) => !IsFloatingPointByte(i, actualPayload.Length) ? "XX" : $"{b:X2}"));
        if (!AreApproximatelyEqual(actualPayload, expectedPayload))
        {
            Assert.Fail($"Float-bytes differ more than 2 units\n" +
                        $"Expected: {expectedFloatBytes}\n" +
                        $"Actual:   {actualFloatBytes}");
        }
    }

    protected abstract TCommand CreateSut(TestUtilities.CommandTests.TestCaseData<TTestData> testCase);
}
