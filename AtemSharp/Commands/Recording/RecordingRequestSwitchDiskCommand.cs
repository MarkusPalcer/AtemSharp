using AtemSharp.Enums;

namespace AtemSharp.Commands.Recording;

[Command("RMSp")]
public class RecordingRequestSwitchDiskCommand : SerializedCommand
{
    public override byte[] Serialize(ProtocolVersion version)
    {
        return [];
    }
}
