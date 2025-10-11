namespace AtemSharp.Enums;

/// <summary>
/// ATEM packet flags for UDP protocol
/// </summary>
[Flags]
public enum PacketFlag : byte
{
    /// <summary>
    /// Request acknowledgment for this packet
    /// </summary>
    AckRequest = 0x01,
    
    /// <summary>
    /// New session ID (used in hello handshake)
    /// </summary>
    NewSessionId = 0x02,
    
    /// <summary>
    /// This packet is a retransmission
    /// </summary>
    IsRetransmit = 0x04,
    
    /// <summary>
    /// Request retransmission from specified packet ID
    /// </summary>
    RetransmitRequest = 0x08,
    
    /// <summary>
    /// Acknowledgment reply
    /// </summary>
    AckReply = 0x10
}