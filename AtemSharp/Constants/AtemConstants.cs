using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.Constants;

/// <summary>
/// ATEM connection constants
/// </summary>
[ExcludeFromCodeCoverage]
public static class AtemConstants
{
    /// <summary>
    /// Default ATEM port
    /// </summary>
    public const int DefaultPort = 9910;

    /// <summary>
    /// Default maximum packet size (matching ATEM software)
    /// </summary>
    internal const int DefaultMaxPacketSize = 1416;

    /// <summary>
    /// Size of ATEM packet header in bytes
    /// </summary>
    internal const int PacketHeaderSize = 12;

    /// <summary>
    /// Size of ATEM command header in bytes
    /// </summary>
    public const ushort CommandHeaderSize = 8;
}
