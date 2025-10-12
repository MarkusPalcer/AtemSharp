namespace AtemSharp.Enums;

/// <summary>
/// ATEM connection states for UDP protocol
/// </summary>
public enum ConnectionState : byte
{
    /// <summary>
    /// No connection established
    /// </summary>
    Closed = 0x00,
    
    SendingSyn = 0xFE,
    
    /// <summary>
    /// Hello packet sent, waiting for response
    /// </summary>
    SynSent = 0x01,
    
    /// <summary>
    /// Connection established and operational
    /// </summary>
    Established = 0x02,
    
    /// <summary>
    /// Disconnected by user (by calling disconnect)
    /// </summary>
    Disconnected = 0x03
}