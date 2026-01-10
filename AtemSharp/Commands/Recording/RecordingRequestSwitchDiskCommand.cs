namespace AtemSharp.Commands.Recording;

/// <summary>
/// Used to request recording to switch between disks
/// </summary>
[Command("RMSp")]
public class RecordingRequestSwitchDiskCommand : EmptyCommand
{
    internal override bool TryMergeTo(SerializedCommand other)
    {
        return other is RecordingRequestSwitchDiskCommand;
    }
}
