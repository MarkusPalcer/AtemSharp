using System.Diagnostics.CodeAnalysis;

namespace AtemSharp;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class ConnectionStateChangedEventArgs(ConnectionState oldState, ConnectionState newState) : EventArgs
{
    public ConnectionState OldState { get; } = oldState;
    public ConnectionState NewState { get; } = newState;
}
