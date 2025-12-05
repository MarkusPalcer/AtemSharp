using AtemSharp.Lib;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Lib;

[TestFixture]
public class ActionLoopTests
{
    [Test]
    public async Task SmokeTest()
    {
        var x = 0;

        var sut = new ActionLoop(async ctx =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), ctx);
            x++;
        }, "SUT");
        sut.Loop();

        Assert.That(sut.IsRunning, Is.True);

        await Task.Delay(TimeSpan.FromMilliseconds(200));
        await sut.Cancel().WithTimeout();
        Assert.That(x, Is.GreaterThan(0));
    }

    [Test]
    public async Task RunAndCancel_NonThrowingCancellation()
    {
        // ReSharper disable once MethodSupportsCancellation
        async Task LoopedAction(CancellationToken token)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(10));
        }

        var sut = new ActionLoop(LoopedAction, "SUT");
        sut.Loop();

        await Task.Delay(TimeSpan.FromMilliseconds(100));

        var cancelTask = sut.Cancel();

        Assert.DoesNotThrowAsync(async () => await cancelTask.WithTimeout());
    }

    [Test]
    public async Task RunAndCancel_ThrowingCancellation()
    {
        // ReSharper disable once MethodSupportsCancellation
        async Task LoopedAction(CancellationToken token)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(10));
            token.ThrowIfCancellationRequested();
        }

        var sut = new ActionLoop(LoopedAction, "SUT");
        sut.Loop();

        await Task.Delay(TimeSpan.FromMilliseconds(100));

        var cancelTask = sut.Cancel();

        Assert.DoesNotThrowAsync(async () => await cancelTask.WithTimeout());
    }

    [Test]
    public async Task RunAndCancel_FailureDuringAction()
    {
        // ReSharper disable once MethodSupportsCancellation
        Task LoopedAction(CancellationToken token)
        {
            throw new ArgumentException();
        }

        var sut = new ActionLoop(LoopedAction, "SUT");
        sut.Loop();

        await Task.Delay(TimeSpan.FromMilliseconds(100));

        var cancelTask = sut.Cancel();

        Assert.DoesNotThrowAsync(async () => await cancelTask.WithTimeout());
    }
}
