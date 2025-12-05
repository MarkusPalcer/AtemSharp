using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace AtemSharp.Lib;

internal class ActionLoop
{
    private readonly Func<CancellationToken, Task> _loopedAction;
    private readonly string _name;
    private readonly ILogger _logger;
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly TaskCompletionSource _taskCompletionSource = new();

    internal bool IsRunning { get; private set; }

    private ActionLoop(Func<CancellationToken, Task> loopedAction, string name, ILogger logger)
    {
        _loopedAction = loopedAction;
        _name = name;
        _logger = logger;
    }

    public class Factory : IActionLoopFactory
    {
        public ActionLoop Start(Func<CancellationToken, Task> loopedAction, ILogger logger,
                                [CallerArgumentExpression(nameof(loopedAction))]
                                string name = null!)
        {
            var result = new ActionLoop(loopedAction, name, logger);
            result.Loop();
            return result;
        }
    }


    private async void Loop()
    {
        _logger.LogDebug("ActionLoop {Name} started", _name);
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
                    _logger.LogError(ex, "ActionLoop {Name} stopped due to exception", _name);
                    return;
                }
            }

            _logger.LogDebug("ActionLoop {Name} finished", _name);
        }
        finally
        {
            IsRunning = false;
            _taskCompletionSource.TrySetResult();
        }
    }

    public async Task Cancel()
    {
        _logger.LogDebug("ActionLoop {Name} shutting down", _name);

        await _cancellationTokenSource.CancelAsync();
        await _taskCompletionSource.Task;

        _logger.LogDebug("ActionLoop {Name} shut down complete", _name);
    }
}
