using AtemSharp.Commands;
using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Tests.Lib.TestCommands;

/// <summary>
/// Demo/test command with no minimum version (baseline)
/// </summary>
[Command("TEST")]
public class TestCommandV1 : IDeserializedCommand
{
	public string Version { get; set; } = "V1";

    public static TestCommandV1 Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
	{
		return new TestCommandV1();
	}

	public void ApplyToState(AtemState state) {}
}
