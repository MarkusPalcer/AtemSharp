using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace AtemSharp.Lib;

internal class ActionLoop
{
    private readonly Func<CancellationToken, Task> _loopedAction;
    private readonly string _name;
    private readonly ILogger _logger;
    private readonly CancellationTokenSource _cancellationTokenSource  = new();
    private readonly TaskCompletionSource _taskCompletionSource = new();

    private ActionLoop(Func<CancellationToken, Task> loopedAction, string name, ILogger logger)
    {
        _loopedAction = loopedAction;
        _name = name;
        _logger = logger;
    }

    public static ActionLoop Start(Func<CancellationToken, Task> loopedAction, ILogger logger, [CallerArgumentExpression(nameof(loopedAction))] string name = null!)
    {
        var result = new ActionLoop(loopedAction, name, logger);
        result.Loop();
        return result;
    }

    public static ActionLoop Start(Action<CancellationToken> loopedAction, ILogger logger, [CallerArgumentExpression(nameof(loopedAction))] string name = null!)
    {
        var result = new ActionLoop(cts =>
        {
            loopedAction(cts);
            return Task.CompletedTask;
        }, name, logger);
        result.Loop();
        return result;
    }

    private async void Loop()
    {
        _logger.LogDebug("ActionLoop {Name} started",  _name);

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
                _taskCompletionSource.TrySetResult();
                _logger.LogError(ex, "ActionLoop {Name} stopped due to exception",  _name);
                return;
            }
        }

        _logger.LogDebug("ActionLoop {Name} finished",  _name);
        _taskCompletionSource.TrySetResult();
    }

    public async Task Cancel()
    {
        _logger.LogDebug("ActionLoop {Name} shutting down",  _name);
        await _cancellationTokenSource.CancelAsync();
        try
        {
            await _taskCompletionSource.Task;
        }
        catch (OperationCanceledException)
        {
            // Expected
        }
        _logger.LogDebug("ActionLoop {Name} shut down complete",  _name);
    }
}
