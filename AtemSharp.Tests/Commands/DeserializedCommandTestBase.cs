using System.Reflection;
using AtemSharp.Commands;

namespace AtemSharp.Tests.Commands;

public abstract class DeserializedCommandTestBase<TCommand, TTestData> : CommandTestBase<TTestData>
	where TCommand : DeserializedCommand
	where TTestData : DeserializedCommandTestBase<TCommand, TTestData>.CommandDataBase, new()
{
	/// <summary>
	/// Override to specify tolerances for floating-point property comparisons
	/// </summary>
	protected virtual double GetFloatingPointTolerance()
	{
		return 0.01; // Default tolerance for decibel values
	}

	/// <summary>
	/// Override to specify which properties contain floating-point values
	/// that should be compared with tolerance
	/// </summary>
	protected virtual string[] GetFloatingPointProperties()
	{
		return Array.Empty<string>();
	}

	protected virtual bool AreApproximatelyEqual(double actual, double expected)
	{
		if (double.IsInfinity(expected) && double.IsInfinity(actual))
			return Math.Sign(expected) == Math.Sign(actual);
		
		return Math.Abs(actual - expected) <= GetFloatingPointTolerance();
	}

	public new class TestCaseData : CommandTestBase<TTestData>.TestCaseData
	{
		// Inherits all properties from base, no additional properties needed
	}

	public new abstract class CommandDataBase : CommandTestBase<TTestData>.CommandDataBase
	{
		// Base class for test data - derived classes add specific properties
	}

	protected static TestCaseData[] LoadTestData()
	{
		// Get the raw name from the CommandAttribute
		var commandAttribute = typeof(TCommand).GetCustomAttribute<CommandAttribute>();
		if (commandAttribute == null)
		{
			throw new InvalidOperationException($"Command {typeof(TCommand).Name} must have a CommandAttribute");
		}
		
		var rawName = commandAttribute.RawName;
		if (string.IsNullOrEmpty(rawName))
		{
			throw new InvalidOperationException($"Command {typeof(TCommand).Name} CommandAttribute must have a RawName");
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
		var commandAttribute = typeof(TCommand).GetCustomAttribute<CommandAttribute>();
		var rawName = commandAttribute?.RawName ?? "Unknown";
		
		Assert.That(testCases.Length, Is.GreaterThan(0), 
		            $"Should have {rawName} test cases from libatem-data.json");

		foreach (var testCase in testCases)
		{
			yield return new NUnit.Framework.TestCaseData(testCase)
			            .SetName($"{rawName}_{testCase.FirstVersion}")
			            .SetDescription(
				             $"Test deserialization for {rawName} with protocol version {testCase.FirstVersion}");
		}
	}

	[Test, TestCaseSource(nameof(GetTestCases))]
	public void TestDeserialization(TestCaseData testCase)
	{
		// Arrange - Extract command payload from the full packet
		var fullPacketBytes = ParseHexBytes(testCase.Bytes);
		var commandPayload = ExtractCommandPayload(fullPacketBytes);

		// Act - Deserialize the command
		var actualCommand = DeserializeCommand(commandPayload);

		// Assert - Compare properties
		CompareCommandProperties(actualCommand, testCase.Command, testCase);
	}

	protected abstract void CompareCommandProperties(TCommand actualCommand, TTestData expectedData, TestCaseData testCase);

	private static TCommand DeserializeCommand(byte[] payload)
	{
		// Use reflection to call the static Deserialize method
		var deserializeMethod = typeof(TCommand).GetMethod("Deserialize", 
		                                                   BindingFlags.Public | BindingFlags.Static,
		                                                   null,
		                                                   new[] { typeof(Stream) },
		                                                   null);

		if (deserializeMethod == null)
		{
			throw new InvalidOperationException($"Command {typeof(TCommand).Name} must have a static Deserialize(Stream) method");
		}

		using var stream = new MemoryStream(payload);
		var result = deserializeMethod.Invoke(null, new object[] { stream });
		
		if (result is not TCommand command)
		{
			throw new InvalidOperationException($"Deserialize method did not return a {typeof(TCommand).Name}");
		}

		return command;
	}

}