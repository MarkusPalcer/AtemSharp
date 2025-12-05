using AtemSharp.Lib;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Lib;

[TestFixture]
public class TaskExtensionsTests
{
    [Test]
    public async Task SucceedingTask()
    {
        var logger = new CapturingLogger();
        var tcs = new TaskCompletionSource();

        var task = logger.WaitForLog();

        tcs.Task.FireAndForget(logger);

        await task.TimesOut();

        tcs.TrySetResult();

        await task.TimesOut();
    }

    [Test]
    public async Task FailingTask()
    {
        var logger = new CapturingLogger();
        var tcs = new TaskCompletionSource();

        var task = logger.WaitForLog();

        tcs.Task.FireAndForget(logger);

        await task.TimesOut();

        tcs.TrySetException(new Exception("Test"));

        await task.WithTimeout();
        Assert.That(logger.CapturedCount, Is.EqualTo(1));
    }

}
