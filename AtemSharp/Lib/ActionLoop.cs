using System.Diagnostics;

namespace AtemSharp.Lib;

internal class ActionLoop(Func<CancellationToken, Task> loopedAction, string name) : IActionLoop
{
    private readonly Func<CancellationToken, Task> _loopedAction = loopedAction;
    private readonly string _name = name;
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly TaskCompletionSource _taskCompletionSource = new();

    internal bool IsRunning { get; private set; }

    public async void Loop()
    {
        Debug.WriteLine($"ActionLoop {_name} started");

        IsRunning = true;

        try
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
                    Debug.Write($"ActionLoop {_name} stopped due to exception\n{ex.Message}\n{ex.StackTrace}");
                    return;
                }
            }

            Debug.WriteLine($"ActionLoop {_name} finished");
        }
        finally
        {
            IsRunning = false;
            _taskCompletionSource.TrySetResult();
        }
    }

    public async Task Cancel()
    {
        Debug.WriteLine($"ActionLoop {_name} shutting down");

        await _cancellationTokenSource.CancelAsync();
        await _taskCompletionSource.Task;

        Debug.WriteLine($"ActionLoop {_name} shut down complete");
    }
}
