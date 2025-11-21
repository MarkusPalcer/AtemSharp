namespace AtemSharp.Communication;

public record AckedPacket(ushort PacketId, int TrackingId);
