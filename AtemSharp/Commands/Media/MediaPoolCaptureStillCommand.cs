using AtemSharp.State.Info;

namespace AtemSharp.Commands.Media;

[Command("Capt")]
public class MediaPoolCaptureStillCommand : SerializedCommand
{
    public override byte[] Serialize(ProtocolVersion version)
    {
        return [];
    }
}
