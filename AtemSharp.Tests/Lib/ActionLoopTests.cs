using AtemSharp.Lib;
using AtemSharp.Logging;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Lib;

[TestFixture]
public class ActionLoopTests
{

    [Test]
    public async Task SmokeTest()
    {
        var x = 0;

        var actionLoop = new ActionLoop.Factory().Start(async ctx =>
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100), ctx);
            x++;
        }, new DebugLoggerFactory().CreateLogger<ActionLoopTests>());

        Assert.That(actionLoop.IsRunning, Is.True);

        await Task.Delay(TimeSpan.FromMilliseconds(200));
        await actionLoop.Cancel().WithTimeout();
        Assert.That(x, Is.GreaterThan(0));
    }
}
