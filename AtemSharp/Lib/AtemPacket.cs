using AtemSharp.Enums;
using System;

namespace AtemSharp.Lib;

/// <summary>
/// Represents an ATEM network packet with header and payload
/// </summary>
public class AtemPacket
{
    private const int PACKET_HEADER_SIZE = 12;
    private const int LENGTH_MASK = 0x07FF; // 11 bits for length
    private const int FLAGS_SHIFT = 11; // Flags are in the upper 5 bits (16 - 11 = 5)

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
    public static int HeaderSize => PACKET_HEADER_SIZE;

    /// <summary>
    /// Creates a new empty ATEM packet
    /// </summary>
    public AtemPacket()
    {
        Payload = Array.Empty<byte>();
    }

    /// <summary>
    /// Creates a new ATEM packet with the specified payload
    /// </summary>
    /// <param name="payload">Payload data</param>
    public AtemPacket(byte[] payload)
    {
        Payload = payload ?? Array.Empty<byte>();
        Length = (ushort)(PACKET_HEADER_SIZE + Payload.Length);
    }

    /// <summary>
    /// Creates a new ATEM packet from raw packet data
    /// </summary>
    /// <param name="packetData">Raw packet bytes</param>
    /// <returns>Parsed ATEM packet</returns>
    /// <exception cref="ArgumentException">Thrown when packet data is invalid</exception>
    public static AtemPacket FromBytes(byte[] packetData)
    {
        if (packetData == null)
            throw new ArgumentNullException(nameof(packetData));

        if (packetData.Length < PACKET_HEADER_SIZE)
            throw new ArgumentException($"Packet too short. Expected at least {PACKET_HEADER_SIZE} bytes, got {packetData.Length}", nameof(packetData));

        var packet = new AtemPacket();

        // Parse header
        ushort flagsAndLength = ReadUInt16BE(packetData, 0);
        packet.Flags = (PacketFlag)(flagsAndLength >> FLAGS_SHIFT);
        packet.Length = (ushort)(flagsAndLength & LENGTH_MASK);
        packet.SessionId = ReadUInt16BE(packetData, 2);
        packet.AckPacketId = ReadUInt16BE(packetData, 4);
        packet.Reserved = ReadUInt16BE(packetData, 6);
        // Skip bytes 8-9 (reserved)
        packet.PacketId = ReadUInt16BE(packetData, 10);

        // Validate length
        if (packet.Length != packetData.Length)
            throw new ArgumentException($"Packet length mismatch. Header indicates {packet.Length} bytes, but received {packetData.Length} bytes", nameof(packetData));

        // Extract payload
        if (packet.Length > PACKET_HEADER_SIZE)
        {
            int payloadLength = packet.Length - PACKET_HEADER_SIZE;
            packet.Payload = new byte[payloadLength];
            Array.Copy(packetData, PACKET_HEADER_SIZE, packet.Payload, 0, payloadLength);
        }
        else
        {
            packet.Payload = Array.Empty<byte>();
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
        Length = (ushort)(PACKET_HEADER_SIZE + Payload.Length);

        var buffer = new byte[Length];

        // Write header
        ushort flagsAndLength = (ushort)(((int)Flags << FLAGS_SHIFT) | (Length & LENGTH_MASK));
        WriteUInt16BE(buffer, 0, flagsAndLength);
        WriteUInt16BE(buffer, 2, SessionId);
        WriteUInt16BE(buffer, 4, AckPacketId);
        WriteUInt16BE(buffer, 6, Reserved);
        // Bytes 8-9 remain zero (reserved)
        WriteUInt16BE(buffer, 10, PacketId);

        // Write payload
        if (Payload.Length > 0)
        {
            Array.Copy(Payload, 0, buffer, PACKET_HEADER_SIZE, Payload.Length);
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
            if (Length != PACKET_HEADER_SIZE + Payload.Length)
                return false;

            // Check minimum length
            if (Length < PACKET_HEADER_SIZE)
                return false;

            // Check maximum reasonable length (64KB should be more than enough)
            if (Length > 65535)
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
            Payload = Array.Empty<byte>()
        };
    }

    /// <summary>
    /// Creates a hello packet for initiating connection
    /// </summary>
    /// <returns>Hello packet</returns>
    public static AtemPacket CreateHello()
    {
        // Use the standard hello packet payload from constants
        var helloPayload = new byte[AtemSharp.Constants.AtemConstants.HELLO_PACKET.Length - PACKET_HEADER_SIZE];
        Array.Copy(AtemSharp.Constants.AtemConstants.HELLO_PACKET, PACKET_HEADER_SIZE, helloPayload, 0, helloPayload.Length);

        return new AtemPacket(helloPayload)
        {
            Flags = PacketFlag.NewSessionId | PacketFlag.AckRequest,
            SessionId = 0,
            PacketId = 1
        };
    }

    private static ushort ReadUInt16BE(byte[] buffer, int offset)
    {
        return (ushort)((buffer[offset] << 8) | buffer[offset + 1]);
    }

    private static void WriteUInt16BE(byte[] buffer, int offset, ushort value)
    {
        buffer[offset] = (byte)(value >> 8);
        buffer[offset + 1] = (byte)(value & 0xFF);
    }

    public override string ToString()
    {
        return $"AtemPacket: Flags={Flags}, Length={Length}, SessionId={SessionId}, PacketId={PacketId}, PayloadSize={Payload.Length}";
    }
}