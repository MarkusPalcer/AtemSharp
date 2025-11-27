using AtemSharp.State.Info;

namespace AtemSharp.Commands.Media;

[Command("Capt")]
public class MediaPoolCaptureStillCommand : SerializedCommand
{
    /// <inheritdoc />
    public override byte[] Serialize(ProtocolVersion version)
    {
        return [];
    }
}
