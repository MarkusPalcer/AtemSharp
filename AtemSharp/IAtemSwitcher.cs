using AtemSharp.Batch;

namespace AtemSharp;

public interface IAtemSwitcher : IAsyncDisposable, IBatchLike
{
    /// <summary>
    /// The state of the connection to the ATEM switcher
    /// </summary>
    ConnectionState ConnectionState { get; }

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

    IBatchOperation StartBatch();
}
