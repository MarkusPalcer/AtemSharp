using System.Net;
using System.Net.Sockets;
using AtemSharp.Commands;
using AtemSharp.Enums;

namespace AtemSharp.Net;

/// <summary>
/// ATEM socket options
/// </summary>
public class AtemSocketOptions
{
    public string Address { get; set; } = "";
    public int Port { get; set; } = AtemConstants.DEFAULT_PORT;
    public bool DebugBuffers { get; set; } = false;
    public bool DisableMultithreaded { get; set; } = false;
    public int ChildProcessTimeout { get; set; } = 600;
    public int MaxPacketSize { get; set; } = AtemConstants.DEFAULT_MAX_PACKET_SIZE;
    public Action<string>? Log { get; set; }
}

/// <summary>
/// ATEM UDP socket connection handler
/// </summary>
public class AtemSocket : AtemSocketEvents, IDisposable
{
    private readonly bool _debugBuffers;
    private readonly bool _disableMultithreaded;
    private readonly int _childProcessTimeout;
    private readonly int _maxPacketSize;
    private readonly CommandParser _commandParser = new();
    private readonly Action<string>? _log;

    private int _nextPacketTrackingId = 0;
    private bool _isDisconnecting = false;
    private string _address = "";
    private int _port = AtemConstants.DEFAULT_PORT;
    
    private AtemSocketChild? _socketProcess;
    private Task? _creatingSocket;
    private bool _disposed = false;

    public AtemSocket(AtemSocketOptions options)
    {
        _debugBuffers = options.DebugBuffers;
        _disableMultithreaded = options.DisableMultithreaded;
        _childProcessTimeout = options.ChildProcessTimeout;
        _maxPacketSize = options.MaxPacketSize;
        _log = options.Log;
        _address = options.Address;
        _port = options.Port;
    }

    /// <summary>
    /// Connect to ATEM device
    /// </summary>
    /// <param name="address">IP address of ATEM</param>
    /// <param name="port">Port number (optional)</param>
    public async Task ConnectAsync(string? address = null, int? port = null)
    {
        _isDisconnecting = false;

        if (address != null)
            _address = address;
        if (port.HasValue)
            _port = port.Value;

        if (_socketProcess == null)
        {
            // Cache the creation task, in case Dispose is called before it completes
            _creatingSocket = CreateSocketProcessAsync();
            await _creatingSocket;

            if (_isDisconnecting || _socketProcess == null)
                throw new InvalidOperationException("Disconnecting");
        }

        await _socketProcess.ConnectAsync(_address, _port);
    }

    /// <summary>
    /// Disconnect from ATEM device
    /// </summary>
    public async Task DisconnectAsync()
    {
        _isDisconnecting = true;

        if (_socketProcess != null)
            await _socketProcess.DisconnectAsync();
    }

    /// <summary>
    /// Dispose of the socket and cleanup resources
    /// </summary>
    public async Task DisposeAsync()
    {
        await DisconnectAsync();

        // Ensure thread creation has finished if it was started
        if (_creatingSocket != null)
        {
            try
            {
                await _creatingSocket;
            }
            catch
            {
                // Ignore errors during disposal
            }
        }

        if (_socketProcess != null)
        {
            await _socketProcess.DisposeAsync();
            _socketProcess = null;
        }
    }

    /// <summary>
    /// Get the next packet tracking ID
    /// </summary>
    public int NextPacketTrackingId
    {
        get
        {
            if (_nextPacketTrackingId >= int.MaxValue)
                _nextPacketTrackingId = 0;
            return ++_nextPacketTrackingId;
        }
    }

    /// <summary>
    /// Send commands to ATEM
    /// </summary>
    /// <param name="commands">Commands to send</param>
    /// <returns>Array of tracking IDs</returns>
    public async Task<int[]> SendCommandsAsync(ISerializableCommand[] commands)
    {
        if (_socketProcess == null)
            throw new InvalidOperationException("Socket not connected");

        return await _socketProcess.SendCommandsAsync(commands);
    }

    private async Task CreateSocketProcessAsync()
    {
        _socketProcess = new AtemSocketChild(new AtemSocketChildOptions
        {
            Address = _address,
            Port = _port,
            DebugBuffers = _debugBuffers
        });

        // Wire up events
        _socketProcess.Disconnect += (_, _) => RaiseDisconnect();
        _socketProcess.Info += (_, e) => RaiseInfo(e.Message);
        _socketProcess.RawDataReceived += (_, e) =>
        {
            var parsedCommands = ParseCommands(e.Payload);
            RaiseReceivedCommands(parsedCommands);
        };
        _socketProcess.AckPackets += (_, e) => RaiseAckPackets(e.TrackingIds);

        await Task.CompletedTask; // Placeholder for async initialization if needed
    }

    private IDeserializedCommand[] ParseCommands(byte[] buffer)
    {
        return _commandParser.ParseCommands(buffer);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            DisposeAsync().GetAwaiter().GetResult();
            _disposed = true;
        }
    }
}