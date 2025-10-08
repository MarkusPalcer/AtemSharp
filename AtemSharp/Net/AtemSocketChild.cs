using System.Net;
using System.Net.Sockets;
using AtemSharp.Commands;

namespace AtemSharp.Net;

/// <summary>
/// Options for AtemSocketChild
/// </summary>
public class AtemSocketChildOptions
{
    public string Address { get; set; } = "";
    public int Port { get; set; } = AtemConstants.DEFAULT_PORT;
    public bool DebugBuffers { get; set; } = false;
}

/// <summary>
/// Child socket class that handles UDP communication with ATEM
/// </summary>
internal class AtemSocketChild : AtemSocketEvents, IAsyncDisposable
{
    private readonly bool _debugBuffers;
    private ConnectionState _connectionState = ConnectionState.Closed;
    private Timer? _reconnectTimer;
    private Timer? _retransmitTimer;

    private int _nextSendPacketId = 1;
    private int _sessionId = 0;

    private string _address = "";
    private int _port;
    private UdpClient? _socket;

    private DateTime _lastReceivedAt = DateTime.UtcNow;
    private int _lastReceivedPacketId = 0;
    private readonly List<InFlightPacket> _inFlight = new();
    private Timer? _ackTimer;
    private bool _ackTimerRunning = false;
    private int _receivedWithoutAck = 0;

    public AtemSocketChild(AtemSocketChildOptions options)
    {
        _debugBuffers = options.DebugBuffers;
        _address = options.Address;
        _port = options.Port;

        CreateSocket();
    }

    /// <summary>
    /// Connect to ATEM device
    /// </summary>
    /// <param name="address">IP address</param>
    /// <param name="port">Port number</param>
    public async Task ConnectAsync(string address, int port)
    {
        _address = address;
        _port = port;

        await RestartConnectionAsync();
    }

    /// <summary>
    /// Disconnect from ATEM device
    /// </summary>
    public async Task DisconnectAsync()
    {
        ClearTimers();

        if (_socket != null)
        {
            _socket.Close();
            _socket.Dispose();
            _socket = null;
        }

        _connectionState = ConnectionState.Disconnected;
        RaiseDisconnect();
        
        await Task.CompletedTask;
    }

    /// <summary>
    /// Send commands to ATEM
    /// </summary>
    /// <param name="commands">Commands to send</param>
    /// <returns>Array of tracking IDs</returns>
    public async Task<int[]> SendCommandsAsync(ISerializableCommand[] commands)
    {
        if (_socket == null)
            throw new InvalidOperationException("Socket not connected");

        var trackingIds = new List<int>();
        
        // TODO: Implement command sending logic using PacketBuilder
        // This is a placeholder implementation
        
        return trackingIds.ToArray();
    }

    private void StartTimers()
    {
        _reconnectTimer ??= new Timer(async _ =>
        {
            if (_lastReceivedAt.AddMilliseconds(NetworkConstants.CONNECTION_TIMEOUT) > DateTime.UtcNow)
            {
                // We heard from the ATEM recently
                return;
            }

            try
            {
                await RestartConnectionAsync();
            }
            catch (Exception e)
            {
                Log($"Reconnect failed: {e.Message}");
            }
        }, null, TimeSpan.FromMilliseconds(NetworkConstants.CONNECTION_RETRY_INTERVAL), 
           TimeSpan.FromMilliseconds(NetworkConstants.CONNECTION_RETRY_INTERVAL));

        // Check for retransmits every 10 milliseconds
        _retransmitTimer ??= new Timer(async _ =>
        {
            try
            {
                await CheckForRetransmitAsync();
            }
            catch (Exception e)
            {
                Log($"Failed to retransmit: {e.Message}");
            }
        }, null, TimeSpan.FromMilliseconds(NetworkConstants.RETRANSMIT_INTERVAL),
           TimeSpan.FromMilliseconds(NetworkConstants.RETRANSMIT_INTERVAL));
    }

    private void ClearTimers()
    {
        _retransmitTimer?.Dispose();
        _retransmitTimer = null;
        
        _reconnectTimer?.Dispose();
        _reconnectTimer = null;
        
        _ackTimer?.Dispose();
        _ackTimer = null;
    }

    private async Task RestartConnectionAsync()
    {
        ClearTimers();

        // This includes a 'disconnect'
        if (_connectionState == ConnectionState.Established)
        {
            _connectionState = ConnectionState.Closed;
            RecreateSocket();
            RaiseDisconnect();
        }
        else if (_connectionState == ConnectionState.Disconnected)
        {
            CreateSocket();
        }

        // Reset connection
        _nextSendPacketId = 1;
        _sessionId = 0;
        _inFlight.Clear();
        Log("reconnect");

        StartTimers();

        // Try doing reconnect
        SendPacket(NetworkConstants.COMMAND_CONNECT_HELLO);
        _connectionState = ConnectionState.SynSent;
        
        await Task.CompletedTask;
    }

    private void Log(string message)
    {
        RaiseInfo(message);
    }

    private void CreateSocket()
    {
        _socket?.Dispose();
        _socket = new UdpClient();
        
        // TODO: Set up socket event handling for received data
        // This would involve listening for incoming UDP packets
        StartReceiving();
    }

    private void RecreateSocket()
    {
        _socket?.Close();
        _socket?.Dispose();
        CreateSocket();
    }

    private void SendPacket(byte[] packet)
    {
        if (_socket == null) return;

        try
        {
            _socket.Send(packet, packet.Length, _address, _port);
        }
        catch (Exception e)
        {
            Log($"Error sending packet: {e.Message}");
        }
    }

    private async void StartReceiving()
    {
        if (_socket == null) return;

        try
        {
            while (_socket != null)
            {
                var result = await _socket.ReceiveAsync();
                ReceivePacket(result.Buffer, result.RemoteEndPoint);
            }
        }
        catch (ObjectDisposedException)
        {
            // Socket was disposed, this is expected
        }
        catch (Exception e)
        {
            Log($"Connection error: {e.Message}");

            if (_connectionState == ConnectionState.Established)
            {
                // If connection is open, then restart. Otherwise the reconnectTimer will handle it
                try
                {
                    await RestartConnectionAsync();
                }
                catch (Exception restartException)
                {
                    Log($"Failed to restartConnection: {restartException.Message}");
                }
            }
        }
    }

    private void ReceivePacket(byte[] packet, IPEndPoint remoteEndPoint)
    {
        _lastReceivedAt = DateTime.UtcNow;
        
        // TODO: Implement packet parsing and handling
        // This involves parsing the ATEM protocol and extracting commands
        // For now, raise the raw data event
        RaiseRawDataReceived(packet);
    }

    private async Task CheckForRetransmitAsync()
    {
        // TODO: Implement retransmission logic
        await Task.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        await DisconnectAsync();
        ClearTimers();
    }
}