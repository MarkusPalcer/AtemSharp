using AtemSharp.Batch;
using AtemSharp.Commands;
using AtemSharp.State;
using AtemSharp.State.Macro;

namespace AtemSharp.Tests.TestUtilities;

public class AtemSwitcherFake : IAtemSwitcher
{
    public readonly List<SerializedCommand> SentCommands = [];

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }

    public AtemState State { get; internal set; } = null!;

    public MacroSystem Macros { get; internal set; } = null!;

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
        throw new  NotImplementedException();
    }

    public Task SendCommandAsync(SerializedCommand command)
    {
        SendCommandsAsync([command]);
        return Task.CompletedTask;
    }

    public Task SendCommandsAsync(IEnumerable<SerializedCommand> commands)
    {
        SentCommands.AddRange(commands);
        return Task.CompletedTask;
    }
}
