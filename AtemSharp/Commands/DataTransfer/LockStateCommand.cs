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
    [SerializedField(0)]
    private ushort _index;

    /// <summary>
    /// Whether the lock should be locked or unlocked
    /// </summary>
    [SerializedField(2)]
    private bool _locked;

    //
    // public ushort Index
    // {
    //     get => _index;
    //     set => _index = value;
    // }
    //
    //
    // public bool Locked
    // {
    //     get => _locked;
    //     set => _locked = value;
    // }
    //
    // /// <summary>
    // /// Create a new lock state command
    // /// </summary>
    // /// <param name="index">Index of the lock</param>
    // /// <param name="locked">Whether the lock should be locked or unlocked</param>
    // public LockStateCommand(ushort index, bool locked)
    // {
    //     Index = index;
    //     Locked = locked;
    // }
    //
    // /// <inheritdoc />
    // public override byte[] Serialize(ProtocolVersion version)
    // {
    //     using var memoryStream = new MemoryStream(4);
    //     using var writer = new BinaryWriter(memoryStream);
    //
    //     writer.WriteUInt16BigEndian(Index);
    //     writer.WriteBoolean(Locked);
    //     writer.Pad(1); // Padding to reach 4 bytes total
    //
    //     return memoryStream.ToArray();
    // }
}
