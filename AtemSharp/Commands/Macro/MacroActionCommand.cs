using AtemSharp.Helpers;
using AtemSharp.Lib;

namespace AtemSharp.Commands.Macro;

[Command("MAct")]
[BufferSize(4)]
public partial class MacroActionCommand(State.Macro macro, MacroAction action) : SerializedCommand
{
    [SerializedField(0)]
    [NoProperty]
    private ushort _index = macro.Id;

    [SerializedField(2)]
    [NoProperty]
    private MacroAction _action = action;

    private void SerializeInternal(byte[] buffer)
    {
        if (_action is MacroAction.Stop or MacroAction.StopRecord or MacroAction.InsertUserWait or MacroAction.Continue)
        {
            buffer.WriteUInt16BigEndian(0xffff, 0);
        }
    }
}

public enum MacroAction : byte {
    Run = 0,
    Stop = 1,
    StopRecord = 2,
    InsertUserWait = 3,
    Continue = 4,
    Delete = 5,
}
