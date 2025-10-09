using AtemSharp.Commands;
using AtemSharp.Enums;

namespace AtemSharp.Tests.Commands;

public abstract class SerializedCommandTestBase<TCommand, TTestData> : CommandTestBase<TTestData>
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

	protected bool IsFloatingPointByte(int index, int totalLength)
	{
		var ranges = GetFloatingPointByteRanges();
		foreach (var range in ranges)
		{
			var (start, length) = range.GetOffsetAndLength(totalLength);
			var end = start + length - 1;
			if (index >= start && index <= end)
				return true;
		}
		return false;
	}

	protected bool AreApproximatelyEqual(byte[] actual, byte[] expected)
	{
		if (actual.Length != expected.Length) return false;
		
		for (int i = 0; i < actual.Length; i++)
		{
			int tolerance = IsFloatingPointByte(i, actual.Length) ? 2 : 0;
			if (Math.Abs(actual[i] - expected[i]) > tolerance)
				return false;
		}
		return true;
	}

	public new class TestCaseData : CommandTestBase<TTestData>.TestCaseData
	{
		// Inherits all properties from base, no additional properties needed
	}

	public new abstract class CommandDataBase : CommandTestBase<TTestData>.CommandDataBase
	{
		public int Mask { get; set; }
	}

	private static TestCaseData[] LoadTestData()
	{
		// Get the raw name from the CommandExtensions and use the base class method
		var rawName = CommandExtensions.GetRawName<TCommand>();
		if (string.IsNullOrEmpty(rawName))
		{
			throw new InvalidOperationException($"Could not determine raw name for command type {typeof(TCommand).Name}");
		}
		
		var baseTestCases = CommandTestBase<TTestData>.LoadTestData(rawName);
		
		// Convert to the derived TestCaseData type
		return baseTestCases.Select(tc => new TestCaseData
		{
			Name = tc.Name,
			FirstVersion = tc.FirstVersion,
			Bytes = tc.Bytes,
			Command = tc.Command
		}).ToArray();
	}

	public static IEnumerable<NUnit.Framework.TestCaseData> GetTestCases()
	{
		var testCases = LoadTestData();
		Assert.That(testCases.Length, Is.GreaterThan(0), "Should have CAMH test cases from libatem-data.json");

		foreach (var testCase in testCases)
		{
			yield return new NUnit.Framework.TestCaseData(testCase)
			            .SetName($"Mask_{testCase.Command.Mask:X2}_{testCase.FirstVersion}")
			            .SetDescription(
				             $"Test serialization for mask {testCase.Command.Mask:X2} with protocol version {testCase.FirstVersion}");
		}
	}

	[Test, TestCaseSource(nameof(GetTestCases))]
	public void TestSerialization(TestCaseData testCase)
	{
		// Arrange - Extract expected payload from the full packet
		var fullPacketBytes = ParseHexBytes(testCase.Bytes);
		var expectedPayload = ExtractCommandPayload(fullPacketBytes, 12); // 12-byte command payload for serialized commands

		var command = CreateSut(testCase);
		command.Flag = (ushort)testCase.Command.Mask;

		// Act
		var actualPayload = command.Serialize(ProtocolVersion.V8_1_1);

		// Assert - First try exact match
		if (actualPayload.SequenceEqual(expectedPayload))
		{
			return;
		}
		
		// Then try approximate match for floating-point fields
		if (AreApproximatelyEqual(actualPayload, expectedPayload))
		{
			return;
		}
		
		Assert.Fail($"Command serialization should match TypeScript reference for mask {testCase.Command.Mask:X2}. " +
		            $"Expected: {BitConverter.ToString(expectedPayload)}, " +
		            $"Actual: {BitConverter.ToString(actualPayload)}");
	}

	protected abstract TCommand CreateSut(TestCaseData testCase);
}