namespace AtemSharp.Net;

/// <summary>
/// ATEM socket connection state
/// </summary>
public enum ConnectionState
{
    Closed = 0x00,
    SynSent = 0x01,
    Established = 0x02,
    /// <summary>
    /// Disconnected by the user (by calling Disconnect())
    /// </summary>
    Disconnected = 0x03,
}

/// <summary>
/// ATEM packet flags
/// </summary>
[Flags]
public enum PacketFlag : byte
{
    AckRequest = 0x01,
    NewSessionId = 0x02,
    IsRetransmit = 0x04,
    RetransmitRequest = 0x08,
    AckReply = 0x10,
}

/// <summary>
/// Information about an outbound packet
/// </summary>
public record OutboundPacketInfo(int PayloadLength, string PayloadHex, int TrackingId);

/// <summary>
/// Information about an in-flight packet awaiting acknowledgment
/// </summary>
internal class InFlightPacket
{
    public int PacketId { get; }
    public int TrackingId { get; }
    public byte[] Payload { get; }
    public DateTime LastSent { get; set; }
    public int Resent { get; set; }

    public InFlightPacket(int packetId, int trackingId, byte[] payload)
    {
        PacketId = packetId;
        TrackingId = trackingId;
        Payload = payload;
        LastSent = DateTime.UtcNow;
        Resent = 0;
    }
}

/// <summary>
/// ATEM connection constants for networking
/// </summary>
internal static class NetworkConstants
{
    public const int IN_FLIGHT_TIMEOUT = 60; // ms
    public const int CONNECTION_TIMEOUT = 5000; // ms
    public const int CONNECTION_RETRY_INTERVAL = 1000; // ms
    public const int RETRANSMIT_INTERVAL = 10; // ms
    public const int MAX_PACKET_RETRIES = 10;
    public const int MAX_PACKET_ID = 1 << 15; // ATEM expects 15 not 16 bits before wrapping
    public const int MAX_PACKET_PER_ACK = 16;

    public static readonly byte[] COMMAND_CONNECT_HELLO = new byte[]
    {
        0x10, 0x14, 0x53, 0xab, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3a, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
    };
}