using AtemSharp.Constants;
using AtemSharp.Enums;

namespace AtemSharp.Lib;

public interface IUdpTransport : IDisposable
{
	/// <summary>
	/// Raised when a packet is received from the remote endpoint
	/// </summary>
	event EventHandler<PacketReceivedEventArgs>? PacketReceived;

	/// <summary>
	/// Raised when the connection state changes
	/// </summary>
	event EventHandler<ConnectionStateChangedEventArgs>? ConnectionStateChanged;

	/// <summary>
	/// Raised when an error occurs during transport operations
	/// </summary>
	event EventHandler<Exception>? ErrorOccurred;

	/// <summary>
	/// Gets the current connection state
	/// </summary>
	ConnectionState ConnectionState { get; }

	/// <summary>
	/// Connects to the specified ATEM device
	/// </summary>
	/// <param name="address">IP address of the ATEM device</param>
	/// <param name="port">Port number (default: 9910)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Task that completes when connected</returns>
	Task ConnectAsync(string address, int port = AtemConstants.DEFAULT_PORT, CancellationToken cancellationToken = default);

	/// <summary>
	/// Disconnects from the ATEM device
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Task that completes when disconnected</returns>
	Task DisconnectAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Sends a packet to the connected ATEM device
	/// </summary>
	/// <param name="packet">Packet to send</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Task that completes when packet is sent</returns>
	Task SendPacketAsync(AtemPacket packet, CancellationToken cancellationToken = default);

	/// <summary>
	/// Sends the initial hello packet to establish connection
	/// </summary>
	Task SendHelloPacketAsync(CancellationToken cancellationToken);
}