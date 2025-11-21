using AtemSharp.Commands;

namespace AtemSharp.Communication;

public interface IAtemSocket : IAsyncDisposable
{
    event EventHandler? Disconnected;
    event EventHandler<ReceivedCommandsEventArgs>? ReceivedCommands;
    event EventHandler<AckPacketsEventArgs>? AckPackets;
    public event EventHandler? Connected;

    Task Connect(string address, int port);
    Task Disconnect();
    int[] SendCommands(SerializedCommand[] commands);
}
