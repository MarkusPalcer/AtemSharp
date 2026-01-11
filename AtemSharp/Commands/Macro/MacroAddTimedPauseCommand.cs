namespace AtemSharp.Commands.Macro;

/// <summary>
/// Used to add a pause to a macro that's being recorded
/// </summary>
[Command("MSlp")]
[BufferSize(4)]
public partial class MacroAddTimedPauseCommand : SerializedCommand
{
    [SerializedField(2)] private ushort _frames;

    internal override bool TryMergeTo(SerializedCommand other)
    {
        if (other is not MacroAddTimedPauseCommand target)
        {
            return false;
        }

        target.Frames += Frames;
        return true;
    }
}
