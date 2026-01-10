using AtemSharp.Batch;
using AtemSharp.Commands;
using AtemSharp.State;
using AtemSharp.State.Macro;

namespace AtemSharp.Tests.TestUtilities;

public class AtemSwitcherFake : IAtemSwitcher
{
    public readonly List<SerializedCommand[]> SendRequests = [];

    public AtemSwitcherFake()
    {
        Macros = new MacroSystem(this);
    }

    public SerializedCommand[] SentCommands => SendRequests.SelectMany(x => x).ToArray();

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }

    public AtemState State { get; internal set; } = null!;

    public MacroSystem Macros { get; internal set; }

    public ConnectionState ConnectionState { get; internal set; } = ConnectionState.Connected;

    public Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task DisconnectAsync()
    {
        return Task.CompletedTask;
    }

    public IBatchOperation StartBatch()
    {
        return new BatchOperation(this);
    }

    public Task SendCommandAsync(SerializedCommand command)
    {
        SendCommandsAsync([command]);
        return Task.CompletedTask;
    }

    public Task SendCommandsAsync(IEnumerable<SerializedCommand> commands)
    {
        SendRequests.Add(commands.ToArray());
        return Task.CompletedTask;
    }
}
