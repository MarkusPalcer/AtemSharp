using AtemSharp.Attributes;
using AtemSharp.Commands;
using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Tests.TestUtilities.TestCommands;

/// <summary>
/// Demo/test command with V8_0 minimum version
/// </summary>
[Command("TEST", ProtocolVersion.V8_0)]
public class TestCommandV2 : IDeserializedCommand
{
	public string Version { get; set; } = "V2";

	public static TestCommandV2 Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
	{
		return new TestCommandV2();
	}

	public void ApplyToState(AtemState state) {}
}
