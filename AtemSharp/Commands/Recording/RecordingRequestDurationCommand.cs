
namespace AtemSharp.Commands.Recording;

/// <summary>
/// Used to request the duration of the current recording
/// </summary>
[Command("RMDR")]
public class RecordingRequestDurationCommand : EmptyCommand
{
    internal override bool TryMergeTo(SerializedCommand other)
    {
        return other is RecordingRequestDurationCommand;
    }
}
