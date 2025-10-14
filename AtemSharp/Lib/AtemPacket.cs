using AtemSharp.Enums;

namespace AtemSharp.Lib;

/// <summary>
/// Represents an ATEM network packet with header and payload
/// </summary>
public class AtemPacket
{
    private const int PacketHeaderSize = 12;
    private const int LengthMask = 0x07FF; // 11 bits for length
    private const int FlagsShift = 11; // Flags are in the upper 5 bits (16 - 11 = 5)

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
    public ushort Reserved { get; set; }

    /// <summary>
    /// Unique packet ID for this packet (15-bit, wraps around)
    /// </summary>
    public ushort PacketId { get; set; }

    /// <summary>
    /// Payload data (commands) - can be empty for control packets
    /// </summary>
    public byte[] Payload { get; set; }

    /// <summary>
    /// Gets the size of the packet header in bytes
    /// </summary>
    public static int HeaderSize => PacketHeaderSize;

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
    /// Tries to parse raw packet data into an ATEM packet
    /// </summary>
    /// <param name="packetData">Raw packet bytes</param>
    /// <param name="packet">Output packet if parsing succeeds</param>
    /// <returns>True if parsing succeeded, false otherwise</returns>
    public static bool TryParse(ReadOnlySpan<byte> packetData, out AtemPacket? packet)
    {
        packet = null;

        try
        {
            packet = FromBytes(packetData);
            return packet.IsValid();
        }
        catch
        {
            return false;
        }
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
            throw new ArgumentException($"Packet too short. Expected at least {PacketHeaderSize} bytes, got {packetData.Length}");

        var packet = new AtemPacket();

        // Parse header using BinaryPrimitives for big-endian reading
        var headerSpan = packetData.Slice(0, PacketHeaderSize);

        ushort flagsAndLength = headerSpan.ReadUInt16BigEndian(0);
        packet.Flags = (PacketFlag)(flagsAndLength >> FlagsShift);
        packet.Length = (ushort)(flagsAndLength & LengthMask);
        packet.SessionId = headerSpan.ReadUInt16BigEndian(2);
        packet.AckPacketId = headerSpan.ReadUInt16BigEndian(4);
        packet.Reserved = headerSpan.ReadUInt16BigEndian(6);
        // Skip bytes 8-9 (reserved)
        packet.PacketId = headerSpan.ReadUInt16BigEndian(10);

        // Validate length
        if (packet.Length != packetData.Length)
            throw new ArgumentException($"Packet length mismatch. Header indicates {packet.Length} bytes, but received {packetData.Length} bytes");

        // Extract payload
        if (packet.Length > PacketHeaderSize)
        {
            var payloadSpan = packetData.Slice(PacketHeaderSize);
            packet.Payload = payloadSpan.ToArray();
        }
        else
        {
            packet.Payload = [];
        }

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
        var bufferSpan = buffer.AsSpan();

        // Write header using BinaryPrimitives for big-endian writing
        ushort flagsAndLength = (ushort)(((int)Flags << FlagsShift) | (Length & LengthMask));
        bufferSpan.WriteUInt16BigEndian(0, flagsAndLength);
        bufferSpan.WriteUInt16BigEndian(2, SessionId);
        bufferSpan.WriteUInt16BigEndian(4, AckPacketId);
        bufferSpan.WriteUInt16BigEndian(6, Reserved);
        // Bytes 8-9 remain zero (reserved)
        bufferSpan.WriteUInt16BigEndian(10, PacketId);

        // Write payload
        if (Payload.Length > 0)
        {
            Payload.AsSpan().CopyTo(bufferSpan.Slice(PacketHeaderSize));
        }

        return buffer;
    }

    /// <summary>
    /// Validates the packet structure and content
    /// </summary>
    /// <returns>True if packet is valid, false otherwise</returns>
    public bool IsValid()
    {
        try
        {
            // Check length consistency
            if (Length != PacketHeaderSize + Payload.Length)
                return false;

            // Check minimum length
            if (Length < PacketHeaderSize)
                return false;

            // Packet structure is valid
            return true;
        }
        catch
        {
            return false;
        }
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

    /// <summary>
    /// Creates a hello packet for initiating connection
    /// </summary>
    /// <returns>Hello packet</returns>
    public static AtemPacket CreateHello()
    {
        // Use the standard hello packet payload from constants
        var helloPayload = new byte[Constants.AtemConstants.HELLO_PACKET.Length - PacketHeaderSize];
        Constants.AtemConstants.HELLO_PACKET.AsSpan(PacketHeaderSize).CopyTo(helloPayload);

        return new AtemPacket(helloPayload)
        {
            Flags = PacketFlag.NewSessionId | PacketFlag.AckRequest,
            SessionId = 0,
            PacketId = 1
        };
    }

    public override string ToString()
    {
        return $"AtemPacket: Flags={Flags}, Length={Length}, SessionId={SessionId}, PacketId={PacketId}, PayloadSize={Payload.Length}";
    }
}
