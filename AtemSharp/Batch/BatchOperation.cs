using System.Diagnostics.CodeAnalysis;
using AtemSharp.Commands;
using AtemSharp.State;
using AtemSharp.State.Macro;

namespace AtemSharp.Batch;

/// <summary>
/// Collects commands and sends them in one operation
/// </summary>
internal class BatchOperation : IBatchOperation
{
    private readonly IBatchLike _source;
    private readonly List<SerializedCommand> _commandsToSend = new();

    public BatchOperation(IBatchLike source)
    {
        _source = source;

        State = _source.State;
        Macros = new MacroSystem(this);

        Revert();
    }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public AtemState State { get; }

    /// <inheritdoc />
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public MacroSystem Macros { get; }

    /// <inheritdoc />
    public Task SendCommandAsync(SerializedCommand command)
    {
        foreach (var cmd in _commandsToSend)
        {
            if (command.TryMergeTo(cmd))
            {
                return Task.CompletedTask;
            }
        }

        _commandsToSend.Add(command);

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task SendCommandsAsync(IEnumerable<SerializedCommand> commands)
    {
        await Task.WhenAll(commands.Select(SendCommandAsync));
    }

    /// <inheritdoc />
    public async Task CommitAsync()
    {
        if (_commandsToSend.Count == 0)
        {
            return;
        }

        await _source.SendCommandsAsync(_commandsToSend);
        _commandsToSend.Clear();
    }

    /// <inheritdoc />
    public void Revert()
    {
        _commandsToSend.Clear();
        _source.Macros.CopyTo(Macros);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        await CommitAsync();
    }
}
