namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Command to set the lock state for a data transfer
/// </summary>
[Command("LOCK")]
[BufferSize(4)]
public partial class LockStateCommand : SerializedCommand
{
    /// <summary>
    /// Index of the lock
    /// </summary>
    [SerializedField(0)] private ushort _index;

    /// <summary>
    /// Whether the lock should be locked or unlocked
    /// </summary>
    [SerializedField(2)] private bool _locked;
}
