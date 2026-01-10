using AtemSharp.Commands;
using AtemSharp.State.Info;

namespace AtemSharp.Tests.TestUtilities.TestCommands;

public class NoRawNameCommand : SerializedCommand
{
    public bool SerializeCalled;

    public override byte[] Serialize(ProtocolVersion version)
    {
        SerializeCalled = true;
        return [];
    }

    internal override bool TryMergeTo(SerializedCommand other)
    {
        throw new NotImplementedException();
    }
}
