using System.Net;
using System.Threading.Tasks.Dataflow;
using AtemSharp.Commands;
using AtemSharp.Communication;
using AtemSharp.Constants;

namespace AtemSharp.Tests.TestUtilities;

public class AtemClientFake : IAtemClient
{
    private IPEndPoint? _remoteEndPoint;

    private TaskCompletionSource _connectTcs = new();
    private TaskCompletionSource _disconnectTcs = new();

    public IPEndPoint? RemoteEndPoint => _remoteEndPoint;

    public List<SerializedCommand> SentCommands { get; } = new();

    public void SuccessfullyConnect()
    {
        _connectTcs.TrySetResult();
        _disconnectTcs = new();
    }

    public void FailConnect(Exception ex) => _connectTcs.SetException(ex);

    public void SuccessfullyDisconnect()
    {
        _disconnectTcs.TrySetResult();
        _connectTcs = new();
    }

    public void FailDisconnect(Exception ex) => _disconnectTcs.SetException(ex);

    IReceivableSourceBlock<IDeserializedCommand> IAtemClient.ReceivedCommands { get; } = new BufferBlock<IDeserializedCommand>();

    async Task IAtemClient.ConnectAsync(string address, int port = AtemConstants.DefaultPort)
    {
        await _connectTcs.Task;

        // Parse the IP address to validate it
        if (!IPAddress.TryParse(address, out var ipAddress))
        {
            throw new ArgumentException("Invalid IP address", nameof(address));
        }

        _remoteEndPoint = new IPEndPoint(ipAddress, port);
    }

    async Task IAtemClient.DisconnectAsync()
    {
        await _disconnectTcs.Task;

        _remoteEndPoint = null;
    }

    Task IAtemClient.SendCommandAsync(SerializedCommand command)
    {
        SentCommands.Add(command);
        return Task.CompletedTask;
    }

    async Task IAtemClient.SendCommandsAsync(IEnumerable<SerializedCommand> commands)
    {
        IAtemClient self = this;
        foreach (var command in commands)
        {
            await self.SendCommandAsync(command);
        }
    }

    public void SimulateReceivedCommand(IDeserializedCommand command)
    {
        IAtemClient self = this;
        self.ReceivedCommands.As<BufferBlock<IDeserializedCommand>>().SendAsync(command);
    }

    ValueTask IAsyncDisposable.DisposeAsync()
    {
        SentCommands.Clear();
        return ValueTask.CompletedTask;
    }
}
