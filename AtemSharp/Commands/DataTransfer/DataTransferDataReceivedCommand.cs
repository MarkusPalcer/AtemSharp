using AtemSharp.State;

namespace AtemSharp.Commands.DataTransfer;

[Command("FTDa")]
internal partial class DataTransferDataReceivedCommand : IDeserializedCommand
{
    [DeserializedField(0)] private ushort _transferId;
    [CustomDeserialization] private byte[] _body = [];

    private void DeserializeInternal(ReadOnlySpan<byte> rawCommand)
    {
        _body = rawCommand.Slice(4, rawCommand.ReadUInt16BigEndian(2)).ToArray();
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Nothing to do - this is just data transport, no state changes
        // The TypeScript implementation also returns an empty array
    }
}
