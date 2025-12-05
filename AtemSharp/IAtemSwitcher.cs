using AtemSharp.Commands;
using AtemSharp.State;

namespace AtemSharp;

public interface IAtemSwitcher : IAsyncDisposable
{
    /// <summary>
    /// The state of the connection to the ATEM switcher
    /// </summary>
    ConnectionState ConnectionState { get; }

    /// <summary>
    /// Gets the current ATEM state
    /// </summary>
    AtemState State { get; }

    /// <summary>
    /// Connects to an ATEM device
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task that completes when connected</returns>
    Task ConnectAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Disconnects from the ATEM device
    /// </summary>
    /// <returns>Task that completes when disconnected</returns>
    Task DisconnectAsync();

    Task SendCommandAsync(SerializedCommand command);
    Task SendCommandsAsync(IEnumerable<SerializedCommand> commands);
}
