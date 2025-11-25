namespace AtemSharp;

public class ConnectionStateChangedEventArgs(ConnectionState oldState, ConnectionState newState) : EventArgs
{
    public ConnectionState OldState { get; } = oldState;
    public ConnectionState NewState { get; } = newState;
}
