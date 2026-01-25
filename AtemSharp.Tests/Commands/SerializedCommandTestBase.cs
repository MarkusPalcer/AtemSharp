using System.Reflection;
using Argon;
using AtemSharp.Attributes;
using AtemSharp.Commands;
using AtemSharp.Tests.Batch;
using AtemSharp.Tests.TestUtilities.CommandTests;
using AtemSharp.Tests.TestUtilities.CommandTests.RecordedTestCases;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace AtemSharp.Tests.Commands;

[TestFixture]
public abstract class SerializedCommandTestBase<TCommand> where TCommand : SerializedCommand
{
    protected abstract TCommand CreateCommand(IStateHolder state, JObject? creationParameters);


    public static IEnumerable<TestCaseData> GetTestCases()
    {
        var commandAttribute = typeof(TCommand).GetCustomAttribute<CommandAttribute>();
        Assert.That(commandAttribute, Is.Not.Null, $"CommandAttribute is required on command class {typeof(TCommand).Name}");

        var rawName = commandAttribute.RawName;

        return RecordedTestCase.GetRecordedTestCases(rawName).Select(testCase => new TestCaseData(testCase).SetName(testCase.Name));
    }

    [Test]
    [TestCaseSource(nameof(GetTestCases))]
    public void TestSerializedCommand(RecordedTestCase testCase)
    {
        // Generate empty state
        var state = Helper.CreateDefaultStateHolder();

        // Create Command
        var command = CreateCommand(state, testCase.CommandCreationParameters);

        // Apply changes to Command
        if (testCase.Changes is not null)
        {
            JsonConvert.PopulateObject(testCase.Changes.ToString(), command);
        }

        var actualPayload = command.Serialize(testCase.Version);
        var expectedPayload = Helper.ParseHexBytes(testCase.Payload);
        Helper.CompareSerializedBytes(actualPayload, expectedPayload, GetFloatingPointByteRanges());
    }

    /// <summary>
    /// Override to specify which byte ranges contain floating-point encoded data
    /// that should be compared with tolerance for precision differences
    /// </summary>
    protected virtual Range[] GetFloatingPointByteRanges()
    {
        return [];
    }

    protected static TestCaseData CreateMergingTestCase<TValue>(
        string propertyName,
        TValue firstValue,
        TValue secondValue)
    {
        return new TestCaseData(propertyName, firstValue, secondValue).SetName(propertyName);
    }

    protected void TestPropertyMerging<TValue>(
        Func<TCommand> factory,
        string propertyName,
        TValue firstValue,
        TValue secondValue)
    {
        var first = factory();
        var second = factory();

        var getter = typeof(TCommand).GetProperty(propertyName)?.GetMethod ??
                     throw new InvalidOperationException($"No getter for property {typeof(TCommand)}.{propertyName} found");
        var setter = typeof(TCommand).GetProperty(propertyName)?.SetMethod ??
                     throw new InvalidOperationException($"No setter for property {typeof(TCommand)}.{propertyName} found");

        setter.Invoke(first, [firstValue]);
        setter.Invoke(second, [secondValue]);

        Assert.That(second.TryMergeTo(first), Is.True);
        Assert.That(getter.Invoke(first, []), Is.EqualTo(secondValue));
    }

    protected void TestPropertyNonMerging<TValue>(
        Func<TCommand> factory,
        string propertyName,
        TValue firstValue,
        TValue secondValue)
    {
        var first = factory();
        var second = factory();

        var getter = typeof(TCommand).GetProperty(propertyName)?.GetMethod ??
                     throw new InvalidOperationException($"No getter for property {typeof(TCommand)}.{propertyName} found");
        var setter = typeof(TCommand).GetProperty(propertyName)?.SetMethod ??
                     throw new InvalidOperationException($"No setter for property {typeof(TCommand)}.{propertyName} found");

        setter.Invoke(first, [firstValue]);

        Assert.That(second.TryMergeTo(first), Is.True);
        Assert.That(getter.Invoke(first, []), Is.EqualTo(firstValue));
    }

    protected void TestPropertyMerging_WithWrongType(Func<TCommand> factory)
    {
        Assert.That(factory().TryMergeTo(new MergeableCommand(2)), Is.False);
    }
}
