using AtemSharp.Lib;

namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Command to request a data transfer download from the ATEM device
/// </summary>
[Command("FTSU")]
[BufferSize(12)]
public partial class DataTransferDownloadRequestCommand : SerializedCommand
{
    /// <summary>
        /// Transfer ID
        /// </summary>
    [SerializedField(0)]
    private ushort _transferId;

    /// <summary>
    /// Transfer store ID
    /// </summary>
    [SerializedField(2)]
    private ushort _transferStoreId;

    /// <summary>
    /// Transfer index
    /// </summary>
    [SerializedField(6)]
    private ushort _transferIndex;

    /// <summary>
    /// Transfer type
    /// </summary>
    [SerializedField(8)]
    private ushort _transferType;


    /// <summary>
    /// Additional unknown field (not supported in TypeScript implementation)
    /// This field exists in the wire format but is not exposed in the TypeScript API
    /// </summary>
    internal ushort Unknown2 { get; init; }

    // Serialize the unknown field to satisfy test cases from TS lib
    // once the value's semantics is known, it will be added to the public interface
    private void SerializeInternal(byte[] buffer)
    {
        buffer.WriteUInt16BigEndian(Unknown2, 10);
    }
}
