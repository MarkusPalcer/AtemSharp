using System.Net;

namespace AtemSharp.Lib;

/// <summary>
/// Event arguments for packet received events
/// </summary>
public class PacketReceivedEventArgs : EventArgs
{
	public required AtemPacket Packet { get; init; }
	public required IPEndPoint RemoteEndPoint { get; init; }
}