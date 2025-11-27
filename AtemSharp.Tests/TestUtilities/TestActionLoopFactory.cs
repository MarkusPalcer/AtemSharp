using AtemSharp.Lib;
using Microsoft.Extensions.Logging;

namespace AtemSharp.Tests.TestUtilities;

internal class TestActionLoopFactory : IActionLoopFactory, IDisposable
{
    private readonly ActionLoop.Factory _actionLoopFactory = new();

    internal readonly List<ActionLoop> RunningLoops = new();

    public ActionLoop Start(Func<CancellationToken, Task> loopedAction, ILogger logger, string name = null)
    {
        var actionLoop = _actionLoopFactory.Start(loopedAction, logger, name);
        RunningLoops.Add(actionLoop);
        return actionLoop;
    }

    public void Dispose()
    {
        Task.WhenAll(RunningLoops.Select(x => x.Cancel())).Wait();
    }
}
