using AtemSharp.Attributes;
using AtemSharp.Commands;
using AtemSharp.State.Info;

namespace AtemSharp.Tests.TestUtilities.TestCommands;

[Command("VSC_")]
public class VariableSizeCommand(byte value, int size) : SerializedCommand
{
    public override byte[] Serialize(ProtocolVersion version)
    {
        return Enumerable.Repeat(value, size).ToArray();
    }

    internal override bool TryMergeTo(SerializedCommand other)
    {
        throw new NotImplementedException();
    }
}
