using AtemSharp.Commands;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Lib.TestCommands;

/// <summary>
/// Demo/test command with no minimum version (baseline)
/// </summary>
[Command("TEST")]
public class TestCommandV1 : IDeserializedCommand
{
	public string Version { get; set; } = "V1";

	public static TestCommandV1 Deserialize(Stream stream, ProtocolVersion protocolVersion)
	{
		return new TestCommandV1();
	}

	public string[] ApplyToState(AtemState state) => ["test"];
}