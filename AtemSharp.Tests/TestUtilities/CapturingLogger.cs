using Microsoft.Extensions.Logging;

namespace AtemSharp.Tests.TestUtilities;

public class CapturingLogger : ILogger
{
    private int _capturedCount;
    private readonly List<TaskCompletionSource> _waitingTasks = new();

    public int CapturedCount
    {
        get => _capturedCount;
        set => _capturedCount = value;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        Interlocked.Increment(ref _capturedCount);

        var tasks = _waitingTasks.ToArray();
        _waitingTasks.Clear();

        foreach (var taskCompletionSource in tasks)
        {
            taskCompletionSource.TrySetResult();
        }
    }

    public bool IsEnabled(LogLevel logLevel) => true;

    public IDisposable BeginScope<TState>(TState state) where TState : notnull => new NullScope();

    private class NullScope : IDisposable
    {
        public void Dispose() { }
    }

    public Task WaitForLog()
    {
        var tcs = new TaskCompletionSource();
        _waitingTasks.Add(tcs);
        return tcs.Task;
    }
}
