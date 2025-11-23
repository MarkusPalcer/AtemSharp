using AtemSharp.Helpers;

namespace AtemSharp.Commands.Macro;

[Command("MSlp")]
[BufferSize(4)]
public partial class MacroAddTimedPauseCommand : SerializedCommand
{
    [SerializedField(2)] private ushort _frames;
}
