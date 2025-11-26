using System.Text;
using AtemSharp.Commands;
using AtemSharp.State.Info;

namespace AtemSharp.Communication;

/// <summary>
/// Helper to build ATEM packet payloads from multiple serialized commands.
/// Mirrors the behavior of the TypeScript PacketBuilder in src/lib/packetBuilder.ts
/// </summary>
public class PacketBuilder
{
    private readonly int _maxPacketSize;
    private readonly ProtocolVersion _protocolVersion;

    private readonly List<byte[]> _completedBuffers = new();

    private bool _finished;
    private byte[] _currentPacketBuffer;
    private int _currentPacketFilled;

    public PacketBuilder(ProtocolVersion protocolVersion)
    {
        _maxPacketSize = Constants.AtemConstants.DefaultMaxPacketSize - Constants.AtemConstants.PacketHeaderSize;
        _protocolVersion = protocolVersion;

        _currentPacketBuffer = new byte[_maxPacketSize];
        _currentPacketFilled = 0;
    }

    public void AddCommand(SerializedCommand cmd)
    {
        if (_finished) throw new InvalidOperationException("Packets have been finished");
        if (cmd == null) throw new ArgumentNullException(nameof(cmd));

        var rawName = cmd.GetRawName();
        if (string.IsNullOrEmpty(rawName) || rawName.Length != 4)
            throw new InvalidOperationException($"Command {cmd.GetType().Name} does not have a valid raw name");

        var payload = cmd.Serialize(_protocolVersion);

        var totalLength = payload.Length + Constants.AtemConstants.CommandHeaderSize; // 8 bytes header

        // If the command itself is larger than a normal packet, finish a buffer sized to the command
        if (totalLength > _maxPacketSize)
        {
            FinishBuffer(totalLength);
        }

        // Ensure the packet will fit into the current buffer
        if (totalLength + _currentPacketFilled > _currentPacketBuffer.Length)
        {
            FinishBuffer();
        }

        // Write command length (big-endian 16-bit)
        _currentPacketBuffer[_currentPacketFilled + 0] = (byte)((totalLength >> 8) & 0xFF);
        _currentPacketBuffer[_currentPacketFilled + 1] = (byte)(totalLength & 0xFF);

        // bytes 2-3 reserved (leave as zero)

        // Write raw name at offset +4 (4 ascii chars)
        var rawNameBytes = Encoding.ASCII.GetBytes(rawName);
        Buffer.BlockCopy(rawNameBytes, 0, _currentPacketBuffer, _currentPacketFilled + 4, 4);

        // Copy payload after the 8-byte header
        if (payload.Length > 0)
        {
            Buffer.BlockCopy(payload, 0, _currentPacketBuffer, _currentPacketFilled + Constants.AtemConstants.CommandHeaderSize, payload.Length);
        }

        _currentPacketFilled += totalLength;
    }

    public IReadOnlyList<byte[]> GetPackets()
    {
        FinishBuffer(0);
        _finished = true;
        return _completedBuffers.AsReadOnly();
    }

    private void FinishBuffer(int newBufferLength = -1)
    {
        if (_finished) return;

        if (_currentPacketFilled > 0)
        {
            var outBuf = new byte[_currentPacketFilled];
            Buffer.BlockCopy(_currentPacketBuffer, 0, outBuf, 0, _currentPacketFilled);
            _completedBuffers.Add(outBuf);
        }

        if (newBufferLength == 0)
        {
            // Do not allocate a new buffer
            _currentPacketBuffer = [];
            _currentPacketFilled = 0;
            return;
        }

        var size = newBufferLength > 0 ? newBufferLength : _maxPacketSize;
        _currentPacketBuffer = new byte[size];
        _currentPacketFilled = 0;
    }
}
