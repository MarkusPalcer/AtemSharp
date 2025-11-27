using AtemSharp.Lib;

namespace AtemSharp.Tests.TestUtilities;

[TestFixture]
public class ActionLoopTests
{
    [Test]
    public async Task RunAndCancel_NonThrowingCancellation()
    {
        var logger = new CapturingLogger();

        // ReSharper disable once MethodSupportsCancellation
        async Task LoopedAction(CancellationToken token)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(10));
        }

        var loop = new ActionLoop.Factory().Start(LoopedAction, logger);

        await Task.Delay(TimeSpan.FromMilliseconds(100));

        var cancelTask = loop.Cancel();

        Assert.DoesNotThrowAsync(async () => await cancelTask.WithTimeout());

        Assert.That(logger.CapturedCount, Is.EqualTo(4));
    }

    [Test]
    public async Task RunAndCancel_ThrowingCancellation()
    {
        var logger = new CapturingLogger();

        // ReSharper disable once MethodSupportsCancellation
        async Task LoopedAction(CancellationToken token)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(10));
            token.ThrowIfCancellationRequested();
        }

        var loop = new ActionLoop.Factory().Start(LoopedAction, logger);

        await Task.Delay(TimeSpan.FromMilliseconds(100));

        var cancelTask = loop.Cancel();

        Assert.DoesNotThrowAsync(async () => await cancelTask.WithTimeout());

        Assert.That(logger.CapturedCount, Is.EqualTo(4));
    }

    [Test]
    public async Task RunAndCancel_FailureDuringAction()
    {
        var logger = new CapturingLogger();

        // ReSharper disable once MethodSupportsCancellation
        async Task LoopedAction(CancellationToken token)
        {
            throw new ArgumentException();
        }

        var loop = new ActionLoop.Factory().Start(LoopedAction, logger);

        await Task.Delay(TimeSpan.FromMilliseconds(100));

        var cancelTask = loop.Cancel();

        Assert.DoesNotThrowAsync(async () => await cancelTask.WithTimeout());
        Assert.That(logger.CapturedCount, Is.EqualTo(4));
    }
}
