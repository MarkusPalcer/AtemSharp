using System.ComponentModel;

namespace AtemSharp.Commands.Macro;


/// <summary>
/// Used to run, stop, delete, etc. a macro
/// </summary>
[Command("MAct")]
[BufferSize(4)]
public partial class MacroActionCommand : SerializedCommand
{
    [SerializedField(0)] [InternalProperty] [ReadOnly(true)] private ushort _index;

    [SerializedField(2)] [InternalProperty] private readonly MacroAction _action;

    private MacroActionCommand(ushort index, MacroAction action)
    {
        _index = index;
        _action = action;
    }

    public static MacroActionCommand Run(State.Macro.Macro macro) => new(macro.Id, MacroAction.Run);
    public static MacroActionCommand Stop() => new(0xffff, MacroAction.Stop);
    public static MacroActionCommand StopRecord() => new(0xffff, MacroAction.StopRecord);
    public static MacroActionCommand InsertUserWait() => new(0xffff, MacroAction.InsertUserWait);
    public static MacroActionCommand Continue() => new(0xffff, MacroAction.Continue);
    public static MacroActionCommand Delete(State.Macro.Macro macro) => new(macro.Id, MacroAction.Delete);

    internal override bool TryMergeTo(SerializedCommand other)
    {
        if (other is not MacroActionCommand target)
        {
            return false;
        }

        if (target.Action != Action)
        {
            return false;
        }

        switch (Action)
        {
            case MacroAction.Run:
                target._index = Index;
                return true;
            case MacroAction.Stop:
            case MacroAction.StopRecord:
            case MacroAction.InsertUserWait:
            case MacroAction.Continue:
                return true;
            case MacroAction.Delete:
                return target._index == _index;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
