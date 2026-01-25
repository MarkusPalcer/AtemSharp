using Argon;
using AtemSharp.Commands.Macro;
using AtemSharp.Tests.Batch;

namespace AtemSharp.Tests.Commands.Macro;

public class MacroAddTimedPauseCommandTests : SerializedCommandTestBase<MacroAddTimedPauseCommand>
{
    protected override MacroAddTimedPauseCommand CreateCommand(IStateHolder state, JObject? creationParameters) => new();

    [Test]
    public void DoesNotMergeWithSameCommand()
    {
        var first = new MacroAddTimedPauseCommand { Frames = 2 };
        var second = new MacroAddTimedPauseCommand { Frames = 3 };

        Assert.That(second.TryMergeTo(first), Is.False);
        Assert.That(first.Frames, Is.EqualTo(2));
    }

    [Test]
    public void DoesNotMergeWithDifferentCommandType()
    {
        var sut = new MacroAddTimedPauseCommand { Frames = 2 };

        Assert.That(sut.TryMergeTo(new MergeableCommand(2)), Is.False);
    }
}

