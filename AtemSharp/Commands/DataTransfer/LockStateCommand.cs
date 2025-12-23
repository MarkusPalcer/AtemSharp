namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Used to set the lock state for a data transfer
/// </summary>
[Command("LOCK")]
[BufferSize(4)]
public partial class LockStateCommand : SerializedCommand
{
    [SerializedField(0)] private ushort _index;
    [SerializedField(2)] private bool _locked;
}
