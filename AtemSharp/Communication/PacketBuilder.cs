using System.Text;
using AtemSharp.Commands;
using AtemSharp.State.Info;

namespace AtemSharp.Communication;

/// <summary>
/// Helper to build ATEM packet payloads from multiple serialized commands.
/// Mirrors the behavior of the TypeScript PacketBuilder in src/lib/packetBuilder.ts
/// </summary>
public class PacketBuilder(ProtocolVersion protocolVersion)
{
    internal const int MaxPacketSize = Constants.AtemConstants.DefaultMaxPacketSize - Constants.AtemConstants.PacketHeaderSize;
    private readonly ProtocolVersion _protocolVersion = protocolVersion;

    private readonly List<byte[]> _completedBuffers = [];

    private byte[] _currentPacketBuffer = new byte[MaxPacketSize];
    private int _currentPacketFilled;

    public void AddCommand(SerializedCommand cmd)
    {
        var rawName = cmd.GetRawName();
        if (string.IsNullOrEmpty(rawName) || rawName.Length != 4)
        {
            throw new InvalidOperationException($"Command {cmd.GetType().Name} does not have a valid raw name");
        }

        var payload = cmd.Serialize(_protocolVersion);

        var totalLength = payload.Length + Constants.AtemConstants.CommandHeaderSize;

        // Ensure the packet will fit into the current buffer
        if (totalLength + _currentPacketFilled > _currentPacketBuffer.Length)
        {
            FinishBuffer();
        }

        // Write command length (big-endian 16-bit)
        _currentPacketBuffer.WriteUInt16BigEndian((ushort)totalLength, _currentPacketFilled);

        // bytes 2-3 reserved (leave as zero)

        // Write raw name at offset +4 (4 ascii chars)
        var rawNameBytes = Encoding.ASCII.GetBytes(rawName);
        Buffer.BlockCopy(rawNameBytes, 0, _currentPacketBuffer, _currentPacketFilled + 4, 4);

        // Copy payload after the 8-byte header
        Buffer.BlockCopy(payload, 0, _currentPacketBuffer, _currentPacketFilled + Constants.AtemConstants.CommandHeaderSize, payload.Length);

        _currentPacketFilled += totalLength;
    }

    public IReadOnlyList<byte[]> GetPackets()
    {
        FinishBuffer();
        var result = _completedBuffers.ToList();
        _completedBuffers.Clear();
        return result.AsReadOnly();
    }

    private void FinishBuffer()
    {
        if (_currentPacketFilled > 0)
        {
            var outBuf = new byte[_currentPacketFilled];
            Buffer.BlockCopy(_currentPacketBuffer, 0, outBuf, 0, _currentPacketFilled);
            _completedBuffers.Add(outBuf);
        }

        _currentPacketBuffer = new byte[MaxPacketSize];
        _currentPacketFilled = 0;
    }
}
