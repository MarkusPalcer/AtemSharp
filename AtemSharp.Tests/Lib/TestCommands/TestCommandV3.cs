using AtemSharp.Commands;
using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Tests.Lib.TestCommands;

/// <summary>
/// Demo/test command with V8_1_1 minimum version
/// </summary>
[Command("TEST", ProtocolVersion.V8_1_1)]
public class TestCommandV3 : IDeserializedCommand
{
	public string Version { get; set; } = "V3";

	public static TestCommandV3 Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
	{
		return new TestCommandV3();
	}

	public void ApplyToState(AtemState state) {}
}
