using AtemSharp.Helpers;

namespace AtemSharp.Commands.Macro;

[Command("MRCP")]
[BufferSize(4)]
public partial class MacroRunStatusCommand : SerializedCommand
{
    [SerializedField(1, 0)] private bool _loop;
}
