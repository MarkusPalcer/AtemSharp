using System.Reactive.Subjects;
using AtemSharp.Commands;
using AtemSharp.Commands.DeviceProfile;
using AtemSharp.Net;
using AtemSharp.State;

namespace AtemSharp;

/// <summary>
/// Options for ATEM connection
/// </summary>
public class AtemOptions
{
    public string? Address { get; set; }
    public int? Port { get; set; }
    public bool? DebugBuffers { get; set; }
    public bool? DisableMultithreaded { get; set; }
    public int? ChildProcessTimeout { get; set; }
    /// <summary>
    /// Maximum size of packets to transmit
    /// </summary>
    public int? MaxPacketSize { get; set; }
}

/// <summary>
/// Event arguments for ATEM state changes
/// </summary>
public class StateChangedEventArgs : EventArgs
{
    public AtemState State { get; }
    public string[] ChangedPaths { get; }

    public StateChangedEventArgs(AtemState state, string[] changedPaths)
    {
        State = state;
        ChangedPaths = changedPaths;
    }
}

/// <summary>
/// Event arguments for commands received
/// </summary>
public class CommandsReceivedAtemEventArgs : EventArgs
{
    public IDeserializedCommand[] Commands { get; }

    public CommandsReceivedAtemEventArgs(IDeserializedCommand[] commands)
    {
        Commands = commands;
    }
}

/// <summary>
/// Basic ATEM connection class
/// </summary>
public class BasicAtem : IDisposable
{
    protected readonly AtemSocket socket;
    private AtemState? _state;
    private readonly Dictionary<string, SentPackets> _sentQueue = new();
    private AtemConnectionStatus _status;

    // Events
    public event EventHandler<string>? Error;
    public event EventHandler<string>? Info;
    public event EventHandler<string>? Debug;
    public event EventHandler? Connected;
    public event EventHandler? Disconnected;
    public event EventHandler<StateChangedEventArgs>? StateChanged;
    public event EventHandler<CommandsReceivedAtemEventArgs>? ReceivedCommands;

    protected virtual void OnError(string message) => Error?.Invoke(this, message);
    protected virtual void OnInfo(string message) => Info?.Invoke(this, message);
    protected virtual void OnDebug(string message) => Debug?.Invoke(this, message);
    protected virtual void OnConnected() => Connected?.Invoke(this, EventArgs.Empty);
    protected virtual void OnDisconnected() => Disconnected?.Invoke(this, EventArgs.Empty);
    protected virtual void OnStateChanged(AtemState state, string[] changedPaths) => StateChanged?.Invoke(this, new StateChangedEventArgs(state, changedPaths));
    protected virtual void OnReceivedCommands(IDeserializedCommand[] commands) => ReceivedCommands?.Invoke(this, new CommandsReceivedAtemEventArgs(commands));

    public BasicAtem(AtemOptions? options = null)
    {
        _state = AtemStateUtil.Create();
        _status = AtemConnectionStatus.Closed;
        
        socket = new AtemSocket(new AtemSocketOptions
        {
            DebugBuffers = options?.DebugBuffers ?? false,
            Address = options?.Address ?? "",
            Port = options?.Port ?? AtemConstants.DEFAULT_PORT,
            DisableMultithreaded = options?.DisableMultithreaded ?? false,
            ChildProcessTimeout = options?.ChildProcessTimeout ?? 600,
            MaxPacketSize = options?.MaxPacketSize ?? AtemConstants.DEFAULT_MAX_PACKET_SIZE,
            Log = message => OnInfo(message)
        });

        socket.ReceivedCommands += (_, e) =>
        {
            OnReceivedCommands(e.Commands);
            MutateState(e.Commands);
        };
        
        socket.AckPackets += (_, e) => ResolveCommands(e.TrackingIds);
        socket.Info += (_, e) => OnInfo(e.Message);
        socket.Debug += (_, e) => OnDebug(e.Message);
        socket.Error += (_, e) => OnError(e.Message);
        socket.Disconnect += (_, _) =>
        {
            _status = AtemConnectionStatus.Closed;
            RejectAllCommands();
            OnDisconnected();
            _state = null;
        };
    }

    /// <summary>
    /// Get the current connection status
    /// </summary>
    public AtemConnectionStatus Status => _status;

    /// <summary>
    /// Get the current ATEM state
    /// </summary>
    public AtemState? State => _state;

    /// <summary>
    /// Connect to ATEM device
    /// </summary>
    /// <param name="address">IP address of ATEM</param>
    /// <param name="port">Port number (optional)</param>
    public async Task ConnectAsync(string address, int? port = null)
    {
        await socket.ConnectAsync(address, port);
    }

    /// <summary>
    /// Disconnect from ATEM device
    /// </summary>
    public async Task DisconnectAsync()
    {
        await socket.DisconnectAsync();
    }

    /// <summary>
    /// Dispose of the connection and cleanup resources
    /// </summary>
    public async Task DestroyAsync()
    {
        await socket.DisposeAsync();
    }

    /// <summary>
    /// Send commands to ATEM
    /// </summary>
    /// <param name="commands">Commands to send</param>
    public async Task SendCommandsAsync(ISerializableCommand[] commands)
    {
        var trackingIds = await socket.SendCommandsAsync(commands);

        var tcs = new TaskCompletionSource<bool>();
        var sentPackets = new SentPackets
        {
            Resolve = () => tcs.SetResult(true),
            Reject = () => tcs.SetException(new InvalidOperationException("Command failed"))
        };

        foreach (var trackingId in trackingIds)
        {
            _sentQueue[trackingId.ToString()] = sentPackets;
        }

        await tcs.Task;
    }

    private void MutateState(IDeserializedCommand[] commands)
    {
        if (_state == null) return;

        var changedPaths = new List<string>();
        var hasInitComplete = false;
        
        foreach (var command in commands)
        {
            if (command is InitCompleteCommand)
            {
                hasInitComplete = true;
                _status = AtemConnectionStatus.Connected;
                OnInitComplete();
            }
            
            var paths = command.ApplyToState(_state);
            changedPaths.AddRange(paths);
        }

        if (hasInitComplete)
        {
            // Connection is now fully established
        }
        else if (_state != null && _status == AtemConnectionStatus.Connected && changedPaths.Count > 0)
        {
            OnStateChanged(_state, changedPaths.ToArray());
        }
    }

    private void OnInitComplete()
    {
        // TODO: Start data transfer manager
        OnConnected();
    }

    private void ResolveCommands(int[] trackingIds)
    {
        foreach (var trackingId in trackingIds)
        {
            var key = trackingId.ToString();
            if (_sentQueue.TryGetValue(key, out var sentPackets))
            {
                sentPackets.Resolve();
                _sentQueue.Remove(key);
            }
        }
    }

    private void RejectAllCommands()
    {
        foreach (var sentPackets in _sentQueue.Values)
        {
            sentPackets.Reject();
        }
        _sentQueue.Clear();
    }

    public void Dispose()
    {
        DestroyAsync().GetAwaiter().GetResult();
    }
}

/// <summary>
/// Full ATEM connection class with additional features
/// </summary>
public class Atem : BasicAtem
{
    public Atem(AtemOptions? options = null) : base(options)
    {
    }

    // TODO: Add additional methods for data transfer, macros, etc.
}

/// <summary>
/// Represents sent packets awaiting acknowledgment
/// </summary>
internal class SentPackets
{
    public required Action Resolve { get; init; }
    public required Action Reject { get; init; }
}