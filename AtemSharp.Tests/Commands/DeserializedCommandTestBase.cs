using System.Reflection;
using AtemSharp.Commands;
using AtemSharp.Enums;
using JetBrains.Annotations;

namespace AtemSharp.Tests.Commands;

public abstract class DeserializedCommandTestBase<TCommand, TTestData> : CommandTestBase<TTestData>
	where TCommand : IDeserializedCommand
	where TTestData : DeserializedCommandTestBase<TCommand, TTestData>.CommandDataBase, new()
{
	private const double FloatingPointTolerance = 0.01;

	/// <summary>
	/// Compares two float values for approximate equality by rounding to the specified number of decimal places.
	/// This is useful when test data has limited precision and exact floating-point comparison is not appropriate.
	/// Use this method when the ATEM binary protocol stores values with specific scaling factors:
	/// - 1 decimal place: Values scaled by 10 (e.g., clip/gain values stored as value * 10)
	/// - 2 decimal places: Values scaled by 100 (e.g., percentage values stored as percentage * 100)
	/// - Higher precision: Values with more complex scaling or conversion factors
	/// </summary>
	/// <param name="actual">The actual value from the command</param>
	/// <param name="expected">The expected value from test data</param>
	/// <param name="decimals">The number of decimal places to round to before comparison</param>
	/// <returns>True if the values are equal after rounding to the specified decimal places</returns>
	protected bool AreApproximatelyEqual(float actual, float expected, int decimals)
	{
		return AreApproximatelyEqual(Math.Round(actual, decimals), Math.Round(expected, decimals), 1.0/(float)Math.Pow(10, decimals+1));
	}

	/// <summary>
	/// Compares two double values for approximate equality by rounding to the specified number of decimal places.
	/// This is useful when test data has limited precision and exact floating-point comparison is not appropriate.
	/// Use this method when the ATEM binary protocol stores values with specific scaling factors:
	/// - 1 decimal place: Values scaled by 10 (e.g., clip/gain values stored as value * 10)
	/// - 2 decimal places: Values scaled by 100 (e.g., percentage values stored as percentage * 100)
	/// - Higher precision: Values with more complex scaling or conversion factors
	/// </summary>
	/// <param name="actual">The actual value from the command</param>
	/// <param name="expected">The expected value from test data</param>
	/// <param name="decimals">The number of decimal places to round to before comparison</param>
	/// <returns>True if the values are equal after rounding to the specified decimal places</returns>
	protected bool AreApproximatelyEqual(double actual, double expected, int decimals)
	{
		return AreApproximatelyEqual(Math.Round(actual, decimals), Math.Round(expected, decimals), 1.0/Math.Pow(10, decimals+1));
	}
	
	/// <summary>
	/// Compares two double values for approximate equality using a fixed tolerance.
	/// </summary>
	/// <param name="actual">The actual value from the command</param>
	/// <param name="expected">The expected value from test data</param>
	/// <param name="tolerance">How much difference between the two numbers is allowed before they are considered non-equal</param>
	/// <returns>True if the values are within the floating-point tolerance</returns>
	protected bool AreApproximatelyEqual(double actual, double expected, double tolerance = FloatingPointTolerance)
	{
		if (double.IsInfinity(expected) && double.IsInfinity(actual))
			return Math.Sign(expected) == Math.Sign(actual);
		
		return Math.Abs(actual - expected) <= tolerance;
	}

	public new class TestCaseData : CommandTestBase<TTestData>.TestCaseData
	{
		// Inherits all properties from base, no additional properties needed
	}

	[UsedImplicitly(ImplicitUseTargetFlags.Members | ImplicitUseTargetFlags.WithInheritors)]
	public new abstract class CommandDataBase : CommandTestBase<TTestData>.CommandDataBase
	{
		// Base class for test data - derived classes add specific properties
	}

	private static TestCaseData[] LoadTestData()
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
		var actualCommand = DeserializeCommand(commandPayload, testCase.FirstVersion);

		// Assert - Compare properties
		CompareCommandProperties(actualCommand, testCase.Command, testCase);
	}

	protected abstract void CompareCommandProperties(TCommand actualCommand, TTestData expectedData, TestCaseData testCase);

	private static TCommand DeserializeCommand(byte[] payload, ProtocolVersion protocolVersion)
	{
		// Use reflection to call the static Deserialize method
		var deserializeMethod = typeof(TCommand).GetMethod("Deserialize", 
		                                                   BindingFlags.Public | BindingFlags.Static,
		                                                   null,
		                                                   [typeof(Stream), typeof(ProtocolVersion)],
		                                                   null);

		if (deserializeMethod == null)
		{
			throw new InvalidOperationException($"Command {typeof(TCommand).Name} must have a static Deserialize(Stream, ProtocolVersion) method");
		}

		using var stream = new MemoryStream(payload);
		var result = deserializeMethod.Invoke(null, [stream, protocolVersion]);
		
		if (result is not TCommand command)
		{
			throw new InvalidOperationException($"Deserialize method did not return a {typeof(TCommand).Name}");
		}

		return command;
	}

}