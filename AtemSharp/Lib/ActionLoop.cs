using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AtemSharp.Lib;

public class ActionLoop
{
    private readonly Func<CancellationToken, Task> _loopedAction;
    private readonly string _name;
    private readonly CancellationTokenSource _cancellationTokenSource  = new();
    private readonly TaskCompletionSource _taskCompletionSource = new();

    private ActionLoop(Func<CancellationToken, Task> loopedAction, string name)
    {
        _loopedAction = loopedAction;
        _name = name;
    }

    public static ActionLoop Start(Func<CancellationToken, Task> loopedAction, [CallerArgumentExpression(nameof(loopedAction))] string name = null!)
    {
        var result = new ActionLoop(loopedAction, name);
        result.Loop();
        return result;
    }

    public static ActionLoop Start(Action<CancellationToken> loopedAction, [CallerArgumentExpression(nameof(loopedAction))] string name = null!)
    {
        var result = new ActionLoop(cts =>
        {
            loopedAction(cts);
            return Task.CompletedTask;
        }, name);
        result.Loop();
        return result;
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
                Debug.Print($"{_name} exception: {ex.Message}");
                return;
            }
        }

        Debug.Print($"{_name} finished.");
        _taskCompletionSource.TrySetResult();
    }

    public async Task Cancel()
    {
        Debug.Print($"Stopping {_name}...");
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
