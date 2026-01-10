namespace AtemSharp.Commands.Macro;


/// <summary>
/// Used to run, stop, delete, etc. a macro
/// </summary>
[Command("MAct")]
[BufferSize(4)]
public partial class MacroActionCommand : SerializedCommand
{
    [SerializedField(0)] [InternalProperty] private readonly ushort _index;

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

        if (target.Index != Index)
        {
            return false;
        }

        return true;
    }
}
