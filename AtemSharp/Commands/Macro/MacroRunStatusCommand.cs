namespace AtemSharp.Commands.Macro;

/// <summary>
/// Used to set whether the macro player should play looped or not
/// </summary>
[Command("MRCP")]
[BufferSize(4)]
public partial class MacroRunStatusCommand : SerializedCommand
{
    [SerializedField(1, 0)] private bool _loop;
}
