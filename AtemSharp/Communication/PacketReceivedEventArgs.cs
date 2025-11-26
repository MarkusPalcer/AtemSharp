namespace AtemSharp.Communication;

/// <summary>
/// Event arguments for packet received events
/// </summary>
public class PacketReceivedEventArgs : EventArgs
{
	public required AtemPacket Packet { get; init; }
}
