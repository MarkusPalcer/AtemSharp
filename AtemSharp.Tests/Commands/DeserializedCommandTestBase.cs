using System.Reflection;
using AtemSharp.Commands;
using AtemSharp.Enums;
using AtemSharp.Lib;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace AtemSharp.Tests.Commands;

public abstract class DeserializedCommandTestBase<TCommand, TTestData> : CommandTestBase<TTestData>
	where TCommand : IDeserializedCommand
	where TTestData : DeserializedCommandTestBase<TCommand, TTestData>.CommandDataBase, new()
{

	[UsedImplicitly(ImplicitUseTargetFlags.Members | ImplicitUseTargetFlags.WithInheritors)]
	public new abstract class CommandDataBase : CommandTestBase<TTestData>.CommandDataBase
	{
		// Base class for test data - derived classes add specific properties
    }


    public static IEnumerable<NUnit.Framework.TestCaseData> GetTestCases()
        => GetTestCases<TCommand>();

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
