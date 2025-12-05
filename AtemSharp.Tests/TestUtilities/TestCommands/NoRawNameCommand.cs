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
}
