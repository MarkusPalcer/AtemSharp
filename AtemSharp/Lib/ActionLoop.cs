namespace AtemSharp.Lib;

public class ActionLoop
{
    private readonly Func<CancellationToken, Task> _loopedAction;
    private readonly CancellationTokenSource _cancellationTokenSource  = new();
    private readonly TaskCompletionSource _taskCompletionSource = new();

    private ActionLoop(Func<CancellationToken, Task> loopedAction)
    {
        _loopedAction = loopedAction;

    }

    public static ActionLoop Start(Func<CancellationToken, Task> loopedAction)
    {
        var result = new ActionLoop(loopedAction);
        result.Loop();
        return result;
    }

    public static ActionLoop Start(Action<CancellationToken> loopedAction)
    {
        return Start(cts =>
        {
            loopedAction(cts);
            return Task.CompletedTask;
        });
    }

    private async void Loop()
    {
        while (!_cancellationTokenSource.IsCancellationRequested)
        {
            try
            {
                await _loopedAction(_cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                // NOP
            }
            catch (Exception ex)
            {
                _taskCompletionSource.TrySetException(ex);
                return;
            }
        }

        _taskCompletionSource.TrySetResult();
    }

    public async Task Cancel()
    {
        await _cancellationTokenSource.CancelAsync();
        try
        {
            await _taskCompletionSource.Task;
        }
        catch (OperationCanceledException)
        {
            // Expected
        }
    }
}
