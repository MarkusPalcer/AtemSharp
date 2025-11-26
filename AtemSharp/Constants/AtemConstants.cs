namespace AtemSharp.Constants;

/// <summary>
/// ATEM connection constants
/// </summary>
public static class AtemConstants
{
    /// <summary>
    /// Default ATEM port
    /// </summary>
    public const int DefaultPort = 9910;

    /// <summary>
    /// Default maximum packet size (matching ATEM software)
    /// </summary>
    public const int DefaultMaxPacketSize = 1416;

    /// <summary>
    /// Size of ATEM packet header in bytes
    /// </summary>
    public const int PacketHeaderSize = 12;

    /// <summary>
    /// Size of ATEM command header in bytes
    /// </summary>
    public const int CommandHeaderSize = 8;

    /// <summary>
    /// Maximum packet ID before wrapping (15-bit, not 16-bit)
    /// </summary>
    public const int MaxPacketId = 1 << 15;

    /// <summary>
    /// Connection timeout in milliseconds
    /// </summary>
    public const int ConnectionTimeoutMs = 5000;

    /// <summary>
    /// Hello packet data for connection initiation
    /// </summary>
    public static readonly byte[] HelloPacket = new byte[]
    {
        0x10, 0x14, 0x53, 0xab, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x3a, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00
    };
}
