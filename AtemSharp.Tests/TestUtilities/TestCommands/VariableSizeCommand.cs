using AtemSharp.Attributes;
using AtemSharp.Commands;
using AtemSharp.State.Info;

namespace AtemSharp.Tests.TestUtilities.TestCommands;

[Command("VSC_")]
public class VariableSizeCommand(int size) : SerializedCommand
{
    public override byte[] Serialize(ProtocolVersion version)
    {
        return new byte[size];
    }
}
