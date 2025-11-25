using AtemSharp.State.Info;

namespace AtemSharp.Commands.Recording;

[Command("RMSp")]
public class RecordingRequestSwitchDiskCommand : SerializedCommand
{
    public override byte[] Serialize(ProtocolVersion version)
    {
        return [];
    }
}
