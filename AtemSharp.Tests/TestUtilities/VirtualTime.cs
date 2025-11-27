using AtemSharp.FrameworkAbstraction;

namespace AtemSharp.Tests.TestUtilities;

public class VirtualTime(DateTime start) : ITimeProvider
{
    public DateTime Now { get; private set; } = start;

    private readonly PriorityQueue<
        (DateTime At, TaskCompletionSource Tcs),
        DateTime> _queue = new();

    Task ITimeProvider.Delay(TimeSpan delay, CancellationToken token)
    {
        var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var scheduled = Now + delay;

        _queue.Enqueue((scheduled, tcs), scheduled);

        token.Register(() => tcs.TrySetCanceled());

        return tcs.Task;
    }

    public async Task AdvanceBy(TimeSpan delta)
        => await AdvanceTo(Now + delta);

    public async Task AdvanceTo(DateTime target)
    {
        while (_queue.Count > 0 && _queue.Peek().At <= target)
        {
            var (at, tcs) = _queue.Dequeue();
            Now = at;
            Assert.That(tcs, Is.Not.Null);
            tcs.TrySetResult();
            await Task.Yield();
        }

        Now = target;
        await Task.Yield();
    }

    public async Task AdvanceByInSteps(TimeSpan delta, TimeSpan step)
     => await AdvanceToInSteps(Now + delta, step);

    private async Task AdvanceToInSteps(DateTime target, TimeSpan step)
    {
        while (Now < target)
        {
            await AdvanceBy(step);
            await Task.Yield();
        }
    }
}

