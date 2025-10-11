using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp;

public class Atem : IDisposable
{
	private readonly CommandParser _commandParser = new();
	private readonly UdpTransport _transport = new();
	private bool _disposed;

	/// <summary>
	/// Gets the current ATEM state
	/// </summary>
	public AtemState? State { get; private set; }

	/// <summary>
	/// Gets a collection of unknown command raw names encountered during communication
	/// </summary>
	public static HashSet<string> UnknownCommands { get; } = new();

	/// <summary>
	/// Command parser for handling ATEM protocol commands
	/// </summary>
	public CommandParser CommandParser => _commandParser;

	/// <summary>
	/// Initializes a new instance of the Atem class
	/// </summary>
	public Atem()
	{
		// Subscribe to transport events
		_transport.PacketReceived += OnPacketReceived;
		_transport.ConnectionStateChanged += OnConnectionStateChanged;
		_transport.ErrorOccurred += OnErrorOccurred;
	}

	/// <summary>
	/// Connects to an ATEM device
	/// </summary>
	/// <param name="remoteHost">IP address of the ATEM device</param>
	/// <param name="remotePort">Port number (default: 9910)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Task that completes when connected</returns>
	public async Task ConnectAsync(string remoteHost, int remotePort = Constants.AtemConstants.DEFAULT_PORT, CancellationToken cancellationToken = default)
	{
		if (_disposed)
			throw new ObjectDisposedException(nameof(Atem));

		State = new AtemState();
		await _transport.ConnectAsync(remoteHost, remotePort, cancellationToken);
	}

	/// <summary>
	/// Disconnects from the ATEM device
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Task that completes when disconnected</returns>
	public async Task DisconnectAsync(CancellationToken cancellationToken = default)
	{
		if (!_disposed)
		{
			await _transport.DisconnectAsync(cancellationToken);
		}
	}

	/// <summary>
	/// Gets the current connection state
	/// </summary>
	public Enums.ConnectionState ConnectionState => _transport.ConnectionState;

	private void OnPacketReceived(object? sender, PacketReceivedEventArgs e)
	{
		// TODO: Parse commands from packet and apply to state
		// For now, this is just a placeholder for the transport functionality
	}

	private void OnConnectionStateChanged(object? sender, ConnectionStateChangedEventArgs e)
	{
		// TODO: Handle connection state changes
		// For now, this is just a placeholder for the transport functionality
	}

	private void OnErrorOccurred(object? sender, Exception e)
	{
		// TODO: Handle transport errors
		// For now, this is just a placeholder for the transport functionality
	}

	/// <summary>
	/// Disposes the Atem instance and releases all resources
	/// </summary>
	public void Dispose()
	{
		if (_disposed)
			return;

		_disposed = true;

		_transport.PacketReceived -= OnPacketReceived;
		_transport.ConnectionStateChanged -= OnConnectionStateChanged;
		_transport.ErrorOccurred -= OnErrorOccurred;
		_transport.Dispose();
	}
}