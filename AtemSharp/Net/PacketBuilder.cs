using System.Text;
using AtemSharp.Commands;
using AtemSharp.Enums;

namespace AtemSharp.Net;

/// <summary>
/// Builds ATEM packets from commands
/// </summary>
public class PacketBuilder
{
    private readonly int _maxPacketSize;
    private readonly ProtocolVersion _protocolVersion;
    private readonly List<byte[]> _completedBuffers = new();
    
    private bool _finished = false;
    private byte[] _currentPacketBuffer;
    private int _currentPacketFilled;

    public PacketBuilder(int maxPacketSize, ProtocolVersion protocolVersion)
    {
        _maxPacketSize = maxPacketSize;
        _protocolVersion = protocolVersion;
        _currentPacketBuffer = new byte[maxPacketSize];
        _currentPacketFilled = 0;
    }

    /// <summary>
    /// Add a command to the packet
    /// </summary>
    /// <param name="command">Command to add</param>
    /// <exception cref="InvalidOperationException">Thrown if packets have been finished</exception>
    public void AddCommand(ISerializableCommand command)
    {
        if (_finished)
            throw new InvalidOperationException("Packets have been finished");

        var rawName = GetCommandRawName(command);
        var payload = command.Serialize(_protocolVersion);

        var totalLength = payload.Length + 8;
        if (totalLength > _maxPacketSize)
        {
            // Command is too large for a normal packet, try sending it on its own anyway
            FinishBuffer(totalLength);
        }

        // Ensure the packet will fit into the current buffer
        if (totalLength + _currentPacketFilled > _currentPacketBuffer.Length)
        {
            FinishBuffer();
        }

        // Add to packet
        WriteUInt16BE(_currentPacketBuffer, _currentPacketFilled + 0, (ushort)(payload.Length + 8));
        Encoding.ASCII.GetBytes(rawName).CopyTo(_currentPacketBuffer, _currentPacketFilled + 4);
        payload.CopyTo(_currentPacketBuffer, _currentPacketFilled + 8);

        _currentPacketFilled += totalLength;
    }

    /// <summary>
    /// Finish building packets and return completed buffers
    /// </summary>
    /// <returns>Array of packet buffers</returns>
    public byte[][] FinishPackets()
    {
        if (!_finished)
        {
            FinishBuffer();
            _finished = true;
        }
        return _completedBuffers.ToArray();
    }

    private void FinishBuffer(int? minSize = null)
    {
        var targetSize = minSize ?? _maxPacketSize;
        if (_currentPacketFilled > 0 || targetSize > _maxPacketSize)
        {
            var actualSize = Math.Max(_currentPacketFilled, targetSize);
            var trimmedBuffer = new byte[actualSize];
            Array.Copy(_currentPacketBuffer, trimmedBuffer, actualSize);
            _completedBuffers.Add(trimmedBuffer);
        }

        _currentPacketBuffer = new byte[_maxPacketSize];
        _currentPacketFilled = 0;
    }

    private static string GetCommandRawName(ISerializableCommand command)
    {
        // Use reflection to get the RawName static property
        var type = command.GetType();
        var rawNameProperty = type.GetProperty("RawName", 
            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
        
        return rawNameProperty?.GetValue(null) as string ?? 
               throw new InvalidOperationException($"Command {type.Name} does not have a RawName property");
    }

    private static void WriteUInt16BE(byte[] buffer, int offset, ushort value)
    {
        buffer[offset] = (byte)(value >> 8);
        buffer[offset + 1] = (byte)(value & 0xFF);
    }
}