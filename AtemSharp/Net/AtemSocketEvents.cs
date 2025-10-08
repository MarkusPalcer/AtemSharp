using System.Reactive.Subjects;
using AtemSharp.Commands;

namespace AtemSharp.Net;

/// <summary>
/// Event arguments for ATEM socket events
/// </summary>
public class AtemSocketEventArgs : EventArgs
{
    public string Message { get; }
    
    public AtemSocketEventArgs(string message)
    {
        Message = message;
    }
}

/// <summary>
/// Event arguments for received commands
/// </summary>
public class CommandsReceivedEventArgs : EventArgs
{
    public IDeserializedCommand[] Commands { get; }
    
    public CommandsReceivedEventArgs(IDeserializedCommand[] commands)
    {
        Commands = commands;
    }
}

/// <summary>
/// Event arguments for raw data received (used internally)
/// </summary>
public class RawDataReceivedEventArgs : EventArgs
{
    public byte[] Payload { get; }
    
    public RawDataReceivedEventArgs(byte[] payload)
    {
        Payload = payload;
    }
}

/// <summary>
/// Event arguments for acknowledged packets
/// </summary>
public class PacketsAcknowledgedEventArgs : EventArgs
{
    public int[] TrackingIds { get; }
    
    public PacketsAcknowledgedEventArgs(int[] trackingIds)
    {
        TrackingIds = trackingIds;
    }
}

/// <summary>
/// ATEM socket events
/// </summary>
public class AtemSocketEvents
{
    public event EventHandler? Disconnect;
    public event EventHandler<AtemSocketEventArgs>? Info;
    public event EventHandler<AtemSocketEventArgs>? Debug;
    public event EventHandler<AtemSocketEventArgs>? Error;
    public event EventHandler<CommandsReceivedEventArgs>? ReceivedCommands;
    public event EventHandler<RawDataReceivedEventArgs>? RawDataReceived;
    public event EventHandler<PacketsAcknowledgedEventArgs>? AckPackets;

    protected virtual void OnDisconnect() => Disconnect?.Invoke(this, EventArgs.Empty);
    protected virtual void OnInfo(string message) => Info?.Invoke(this, new AtemSocketEventArgs(message));
    protected virtual void OnDebug(string message) => Debug?.Invoke(this, new AtemSocketEventArgs(message));
    protected virtual void OnError(string message) => Error?.Invoke(this, new AtemSocketEventArgs(message));
    protected virtual void OnReceivedCommands(IDeserializedCommand[] commands) => ReceivedCommands?.Invoke(this, new CommandsReceivedEventArgs(commands));
    protected virtual void OnRawDataReceived(byte[] payload) => RawDataReceived?.Invoke(this, new RawDataReceivedEventArgs(payload));
    protected virtual void OnAckPackets(int[] trackingIds) => AckPackets?.Invoke(this, new PacketsAcknowledgedEventArgs(trackingIds));
    
    // Internal methods for derived classes
    internal void RaiseDisconnect() => OnDisconnect();
    internal void RaiseInfo(string message) => OnInfo(message);
    internal void RaiseDebug(string message) => OnDebug(message);
    internal void RaiseError(string message) => OnError(message);
    internal void RaiseReceivedCommands(IDeserializedCommand[] commands) => OnReceivedCommands(commands);
    internal void RaiseRawDataReceived(byte[] payload) => OnRawDataReceived(payload);
    internal void RaiseAckPackets(int[] trackingIds) => OnAckPackets(trackingIds);
}