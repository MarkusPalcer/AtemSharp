using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.Communication;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
internal struct InFlightPacket(ushort packetId, int trackingId, byte[] payload)
{
    public readonly ushort PacketId = packetId;
    public readonly int TrackingId = trackingId;
    public readonly byte[] Payload = payload;
    public DateTime LastSent;
    public uint Resent;
    public readonly bool NonDefault = true;
}
