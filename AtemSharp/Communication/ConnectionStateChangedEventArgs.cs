namespace AtemSharp.Lib;

/// <summary>
/// Event arguments for connection state change events
/// </summary>
public class ConnectionStateChangedEventArgs : EventArgs
{
	public required ConnectionState State { get; init; }
	public required ConnectionState PreviousState { get; init; }
}