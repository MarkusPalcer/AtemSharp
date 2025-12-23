namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Used to request a data transfer download from the ATEM device
/// </summary>
[Command("FTSU")]
[BufferSize(12)]
public partial class DataTransferDownloadRequestCommand : SerializedCommand
{
    [SerializedField(0)] private ushort _transferId;

    [SerializedField(2)] private ushort _transferStoreId;

    [SerializedField(6)] private ushort _transferIndex;

    [SerializedField(8)] private ushort _transferType;

    internal ushort Unknown2 { get; init; }

    // Serialize the unknown field to satisfy test cases from TS lib
    // once the value's semantics is known, it will be added to the public interface
    private void SerializeInternal(byte[] buffer)
    {
        buffer.WriteUInt16BigEndian(Unknown2, 10);
    }
}
