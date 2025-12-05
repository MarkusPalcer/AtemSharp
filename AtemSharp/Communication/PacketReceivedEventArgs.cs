using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.Communication;

/// <summary>
/// Event arguments for packet received events
/// </summary>
[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class PacketReceivedEventArgs : EventArgs
{
	public required AtemPacket Packet { get; init; }
}
