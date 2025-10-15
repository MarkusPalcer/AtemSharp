using System.Reflection;
using AtemSharp.Commands;
using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.Tests.TestUtilities;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AtemSharp.Tests.Commands;

public abstract class DeserializedCommandTestBase<TCommand, TTestData> : CommandTestBase<TTestData>
	where TCommand : IDeserializedCommand
	where TTestData : DeserializedCommandTestBase<TCommand, TTestData>.CommandDataBase, new()
{

	public new class TestCaseData : CommandTestBase<TTestData>.TestCaseData
	{
		// Inherits all properties from base, no additional properties needed
	}

	[UsedImplicitly(ImplicitUseTargetFlags.Members | ImplicitUseTargetFlags.WithInheritors)]
	public new abstract class CommandDataBase : CommandTestBase<TTestData>.CommandDataBase
	{
		// Base class for test data - derived classes add specific properties

        [JsonExtensionData] public Dictionary<string, JToken> UnknownProperties { get; set; } = new();
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
        Assert.That(commandAttribute, Is.Not.Null, $"CommandAttribute is required on command class {typeof(TCommand).Name}");
        var minProtocolVersion = commandAttribute.MinimumVersion;
		var rawName = commandAttribute.RawName ?? "Unknown";

		Assert.That(testCases.Length, Is.GreaterThan(0),
		            $"Should have {rawName} test cases from libatem-data.json");

        var maxProtocolVersion = typeof(TTestData).GetCustomAttribute<MaxProtocolVersionAttribute>()?.MaxVersion;

		foreach (var testCase in testCases)
		{
            if (maxProtocolVersion is not null && testCase.FirstVersion > maxProtocolVersion)
            {
                continue;
            }

            if (testCase.FirstVersion < minProtocolVersion)
            {
                continue;
            }

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

        if (testCase.Command.UnknownProperties.Count != 0)
        {
            Assert.Fail("Unprocessed test data:\n" + JsonConvert.SerializeObject(testCase.Command.UnknownProperties));
        }
	}

    public ProtocolVersion? MaxProtocolVersion { get; protected set; }

	protected abstract void CompareCommandProperties(TCommand actualCommand, TTestData expectedData, TestCaseData testCase);

	private static TCommand DeserializeCommand(byte[] payload, ProtocolVersion protocolVersion)
	{
		// Use reflection to call the static Deserialize method
		var deserializeMethod = typeof(TCommand).GetMethod("Deserialize",
		                                                   BindingFlags.Public | BindingFlags.Static,
		                                                   null,
		                                                   [typeof(ReadOnlySpan<Byte>), typeof(ProtocolVersion)],
		                                                   null);

		if (deserializeMethod == null)
		{
			throw new InvalidOperationException($"Command {typeof(TCommand).Name} is missing public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> data, ProtocolVersion version)");
		}

        var result = deserializeMethod.CreateDelegate<CommandParser.DeserializeCommand>()(payload.AsSpan(), protocolVersion);

		if (result is not TCommand command)
		{
			throw new InvalidOperationException($"Deserialize method did not return a {typeof(TCommand).Name}");
		}

		return command;
	}

}
