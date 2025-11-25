using AtemSharp.State.Info;

namespace AtemSharp.Commands;

[Command("TiRq", ProtocolVersion.V8_0)]
public class TimeRequestCommand : SerializedCommand
{
    public override byte[] Serialize(ProtocolVersion version)
    {
        return [];
    }
}
