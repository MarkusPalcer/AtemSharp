namespace AtemSharp.Constants;

/// <summary>
/// ATEM connection constants
/// </summary>
public static class AtemConstants
{
    /// <summary>
    /// Default ATEM port
    /// </summary>
    public const int DEFAULT_PORT = 9910;
    
    /// <summary>
    /// Default maximum packet size (matching ATEM software)
    /// </summary>
    public const int DEFAULT_MAX_PACKET_SIZE = 1416;
    
    /// <summary>
    /// Size of ATEM packet header in bytes
    /// </summary>
    public const int PACKET_HEADER_SIZE = 12;
    
    /// <summary>
    /// Size of ATEM command header in bytes
    /// </summary>
    public const int COMMAND_HEADER_SIZE = 8;
    
    /// <summary>
    /// Maximum packet ID before wrapping (15-bit, not 16-bit)
    /// </summary>
    public const int MAX_PACKET_ID = 1 << 15;
    
    /// <summary>
    /// Connection timeout in milliseconds
    /// </summary>
    public const int CONNECTION_TIMEOUT_MS = 5000;
    
    /// <summary>
    /// Hello packet data for connection initiation
    /// </summary>
    public static readonly byte[] HELLO_PACKET = new byte[]
    {
        0x10, 0x14, 0x53, 0xab, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x3a, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00
    };
}