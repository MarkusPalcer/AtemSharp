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
		try
		{
			// Extract commands from packet payload and apply to state
			// Based on TypeScript atemSocket.ts _parseCommands method (lines 181-217)
			var payload = e.Packet.Payload;
			var offset = 0;

			// Parse all commands in the packet payload
			while (offset + Constants.AtemConstants.COMMAND_HEADER_SIZE <= payload.Length)
			{
				// Extract command header (8 bytes: length, reserved, rawName)
				var commandLength = (payload[offset] << 8) | payload[offset + 1]; // Big-endian 16-bit
				// Skip reserved bytes (offset + 2, offset + 3)
				var rawName = System.Text.Encoding.ASCII.GetString(payload, offset + 4, 4);

				// Validate command length
				if (commandLength < Constants.AtemConstants.COMMAND_HEADER_SIZE)
				{
					// Commands are never less than 8 bytes (header size)
					break;
				}

				if (offset + commandLength > payload.Length)
				{
					// Command extends beyond payload - malformed packet
					break;
				}

				// Extract command data (excluding the 8-byte header)
				var commandDataStart = offset + Constants.AtemConstants.COMMAND_HEADER_SIZE;
				var commandDataLength = commandLength - Constants.AtemConstants.COMMAND_HEADER_SIZE;
				using var commandDataStream = new MemoryStream(payload, commandDataStart, commandDataLength);

				try
				{
					// Try to parse the command using CommandParser
					var command = _commandParser.ParseCommand(rawName, commandDataStream);
					if (command != null)
					{
						// Apply the command to the current state
						command.ApplyToState(State!);
					}
					// Note: Unknown commands are tracked by CommandParser.ParseCommand
				}
				catch (Exception ex)
				{
					// Log command parsing error but continue processing other commands
					// Matches TypeScript emit('error', `Failed to deserialize command: ${cmdConstructor.constructor.name}: ${e}`)
					Console.WriteLine($"Failed to deserialize command {rawName}: {ex.Message}");
				}

				// Move to next command
				offset += commandLength;
			}
		}
		catch (Exception ex)
		{
			// Handle any unexpected errors during packet processing
			Console.WriteLine($"Error processing packet: {ex.Message}");
		}
	}

	private void OnConnectionStateChanged(object? sender, ConnectionStateChangedEventArgs e)
	{
		// Handle connection state transitions
		// This is primarily driven by the transport layer (UDP handshake)
		// but may be further refined by command processing (e.g., InitComplete)
		
		Console.WriteLine($"Connection state changed: {e.PreviousState} -> {e.State}");
	}

	private void OnErrorOccurred(object? sender, Exception e)
	{
		// Handle transport layer errors
		Console.WriteLine($"Transport error occurred: {e.Message}");
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