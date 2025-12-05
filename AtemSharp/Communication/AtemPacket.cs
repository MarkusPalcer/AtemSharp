using AtemSharp.Commands;

namespace AtemSharp.Communication;

/// <summary>
/// Represents an ATEM network packet with header and payload
/// </summary>
public class AtemPacket
{
    private const int PacketHeaderSize = 12;
    private const int LengthMask = 0x07FF; // 11 bits for length

    /// <summary>
    /// Packet flags indicating the type and properties of the packet
    /// </summary>
    public PacketFlag Flags { get; set; }

    /// <summary>
    /// Total length of the packet including header (12 bytes + payload length)
    /// </summary>
    public ushort Length { get; set; }

    /// <summary>
    /// Session ID assigned by the ATEM device
    /// </summary>
    public ushort SessionId { get; set; }

    /// <summary>
    /// Packet ID being acknowledged (used in ACK packets)
    /// </summary>
    public ushort AckPacketId { get; set; }

    /// <summary>
    /// Reserved field or packet ID to resend from (used in retransmit requests)
    /// </summary>
    public ushort RetransmitFromPacketId { get; set; }

    /// <summary>
    /// Unique packet ID for this packet (15-bit, wraps around)
    /// </summary>
    public ushort PacketId { get; set; }

    /// <summary>
    /// Payload data (commands) - can be empty for control packets
    /// </summary>
    public byte[] Payload { get; set; }

    /// <summary>
    /// Internally used ID to track ACKs of packets
    /// </summary>
    public int TrackingId { get; set; }

    /// <summary>
    /// Creates a new empty ATEM packet
    /// </summary>
    public AtemPacket()
    {
        Payload = [];
    }

    /// <summary>
    /// Creates a new ATEM packet with the specified payload
    /// </summary>
    /// <param name="payload">Payload data</param>
    public AtemPacket(byte[] payload)
    {
        Payload = payload;
        Length = (ushort)(PacketHeaderSize + Payload.Length);
    }

    /// <summary>
    /// Creates a new ATEM packet from raw packet data
    /// </summary>
    /// <param name="packetData">Raw packet bytes</param>
    /// <returns>Parsed ATEM packet</returns>
    /// <exception cref="ArgumentException">Thrown when packet data is invalid</exception>
    public static AtemPacket FromBytes(ReadOnlySpan<byte> packetData)
    {
        if (packetData.Length < PacketHeaderSize)
        {
            throw new ArgumentException($"Packet too short. Expected at least {PacketHeaderSize} bytes, got {packetData.Length}");
        }

        var packet = new AtemPacket();

        // Parse header using BinaryPrimitives for big-endian reading
        var headerSpan = packetData.Slice(0, PacketHeaderSize);

        // Extract flags and length following TypeScript implementation
        // Flags are in the upper 5 bits of the first byte
        // Length is in the lower 11 bits of the first two bytes
        var flagsAndLength = headerSpan.ReadUInt16BigEndian(0);

        // Stryker disable once bitwise: >> and >>> do the same for unsigned types
        packet.Flags = (PacketFlag)(headerSpan.ReadUInt8(0) >> 3);

        packet.Length = (ushort)(flagsAndLength & LengthMask);
        packet.SessionId = headerSpan.ReadUInt16BigEndian(2);
        packet.AckPacketId = headerSpan.ReadUInt16BigEndian(4);
        packet.RetransmitFromPacketId = headerSpan.ReadUInt16BigEndian(6);
        // Skip bytes 8-9 (reserved)
        packet.PacketId = headerSpan.ReadUInt16BigEndian(10);

        // Validate length
        if (packet.Length != packetData.Length)
        {
            throw new ArgumentException($"Packet length mismatch. Header indicates {packet.Length} bytes, but received {packetData.Length} bytes");
        }

        packet.Payload = packetData[PacketHeaderSize..].ToArray();

        return packet;
    }

    /// <summary>
    /// Converts the packet to raw bytes for network transmission
    /// </summary>
    /// <returns>Packet as byte array</returns>
    public byte[] ToBytes()
    {
        // Update length to match current payload
        Length = (ushort)(PacketHeaderSize + Payload.Length);

        var buffer = new byte[Length];

        // Write header using BinaryPrimitives for big-endian writing
        // Encode flags in upper 5 bits of first byte, length in lower 11 bits
        // Stryker disable once bitwise: >> and >>> do the same for unsigned types
        buffer[0] = (byte)(((int)Flags << 3) | ((Length >> 8) & 0x07)); // First byte: upper 5 bits flags + upper 3 bits of length
        buffer[1] = (byte)(Length & 0xFF); // Second byte: lower 8 bits of length
        buffer.WriteUInt16BigEndian(SessionId, 2);
        buffer.WriteUInt16BigEndian(AckPacketId, 4);
        buffer.WriteUInt16BigEndian(RetransmitFromPacketId, 6);
        // Bytes 8-9 remain zero (reserved)
        buffer.WriteUInt16BigEndian(PacketId, 10);

        Payload.CopyTo(buffer, PacketHeaderSize);

        return buffer;
    }

    /// <summary>
    /// Checks if this packet has the specified flag set
    /// </summary>
    /// <param name="flag">Flag to check</param>
    /// <returns>True if flag is set</returns>
    public bool HasFlag(PacketFlag flag)
    {
        return (Flags & flag) != 0;
    }

    /// <summary>
    /// Creates an ACK packet for acknowledging the specified packet ID
    /// </summary>
    /// <param name="sessionId">Session ID</param>
    /// <param name="packetIdToAck">Packet ID to acknowledge</param>
    /// <returns>ACK packet</returns>
    public static AtemPacket CreateAck(ushort sessionId, ushort packetIdToAck)
    {
        return new AtemPacket
        {
            Flags = PacketFlag.AckReply,
            SessionId = sessionId,
            AckPacketId = packetIdToAck,
            PacketId = 0, // ACK packets don't need their own packet ID
            Payload = []
        };
    }

    public override string ToString()
    {
        return $"AtemPacket: Flags={Flags}, Length={Length}, SessionId={SessionId}, PacketId={PacketId}, PayloadSize={Payload.Length}";
    }
}
