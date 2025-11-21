namespace AtemSharp.Communication;

public struct InFlightPacket
{
    public readonly ushort PacketId;
    public readonly int TrackingId;
    public readonly byte[] Payload;
    public DateTime LastSent;
    public uint Resent;
    public readonly bool NonDefault = true;

    public InFlightPacket(ushort packetId, int trackingId, byte[] payload)
    {
        PacketId = packetId;
        TrackingId = trackingId;
        Payload = payload;
    }
}
