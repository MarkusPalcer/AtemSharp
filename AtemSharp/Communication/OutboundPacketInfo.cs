namespace AtemSharp.Communication;

public record OutboundPacketInfo(byte[] Payload, int TrackingId);
