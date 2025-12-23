using AtemSharp.Commands;

namespace AtemSharp.Communication;

internal interface IAtemSocket : IAsyncDisposable
{
    event EventHandler? Disconnected;
    event EventHandler<ReceivedCommandsEventArgs>? ReceivedCommands;
    public event EventHandler? Connected;

    Task Connect(string address, int port);
    Task Disconnect();
    Task SendCommands(SerializedCommand[] commands);
}
