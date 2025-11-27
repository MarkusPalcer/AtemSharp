using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.Communication;

/// <summary>
/// Event arguments for connection state change events
/// </summary>
[ExcludeFromCodeCoverage]
public class ConnectionStateChangedEventArgs : EventArgs
{
	public required AtemSharp.ConnectionState State { get; init; }
	public required AtemSharp.ConnectionState PreviousState { get; init; }
}
