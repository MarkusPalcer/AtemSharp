using AtemSharp.State.Info;

namespace AtemSharp.Commands.Recording;

[Command("RMSp")]
public class RecordingRequestSwitchDiskCommand : SerializedCommand
{
    /// <inheritdoc />
    public override byte[] Serialize(ProtocolVersion version)
    {
        return [];
    }
}
