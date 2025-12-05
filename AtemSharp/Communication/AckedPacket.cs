using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.Communication;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public record AckedPacket(ushort PacketId, int TrackingId);
