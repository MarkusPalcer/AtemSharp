namespace AtemSharp;

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
}

/// <summary>
/// ATEM connection status
/// </summary>
public enum AtemConnectionStatus
{
    Closed,
    Connecting,
    Connected,
}