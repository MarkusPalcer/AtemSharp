namespace AtemSharp.Commands.Macro;

[Command("MAct")]
[BufferSize(4)]
public partial class MacroActionCommand(State.Macro.Macro macro, MacroAction action) : SerializedCommand
{
    [SerializedField(0)] [NoProperty] private readonly ushort _index = macro.Id;

    [SerializedField(2)] [NoProperty] private readonly MacroAction _action = action;

    private void SerializeInternal(byte[] buffer)
    {
        if (_action is MacroAction.Stop or MacroAction.StopRecord or MacroAction.InsertUserWait or MacroAction.Continue)
        {
            buffer.WriteUInt16BigEndian(0xffff, 0);
        }
    }
}
