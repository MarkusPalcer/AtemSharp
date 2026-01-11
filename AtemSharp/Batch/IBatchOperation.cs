namespace AtemSharp.Batch;

public interface IBatchOperation : IBatchLike, IAsyncDisposable
{
    /// <summary>
    /// Sends all commands that are queued in one operation
    /// </summary>
    Task CommitAsync();

    /// <summary>
    /// Removes all queued commands and updates the state from the real device
    /// </summary>
    void Revert();
}
