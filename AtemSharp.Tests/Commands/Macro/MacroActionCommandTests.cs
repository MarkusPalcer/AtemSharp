using AtemSharp.Commands.Macro;
using AtemSharp.State.Macro;
using AtemSharp.Tests.Batch;
using NSubstitute;

namespace AtemSharp.Tests.Commands.Macro;

public class MacroActionCommandTests : SerializedCommandTestBase<MacroActionCommand, MacroActionCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }

        public MacroAction Action { get; set; }
    }

    protected override MacroActionCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        var macro = new AtemSharp.State.Macro.Macro(Substitute.For<IAtemSwitcher>()) { Id = testCase.Command.Index };
        return testCase.Command.Action switch
        {
            MacroAction.Run => MacroActionCommand.Run(macro),
            MacroAction.Stop => MacroActionCommand.Stop(),
            MacroAction.StopRecord => MacroActionCommand.StopRecord(),
            MacroAction.InsertUserWait => MacroActionCommand.InsertUserWait(),
            MacroAction.Continue => MacroActionCommand.Continue(),
            MacroAction.Delete => MacroActionCommand.Delete(macro),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    [Test]
    public void LatestRunWins()
    {
        var state = new MacroSystem(Substitute.For<IAtemSwitcher>());
        state.Populate(5);

        var first = MacroActionCommand.Run(state[0]);
        var second = MacroActionCommand.Run(state[1]);

        Assert.That(second.TryMergeTo(first), Is.True);
        Assert.Multiple(() =>
        {
            Assert.That(first.Action, Is.EqualTo(MacroAction.Run));
            Assert.That(first.Index, Is.EqualTo(1));
        });
    }

    [Test]
    public void MergeStop()
    {
        var state = new MacroSystem(Substitute.For<IAtemSwitcher>());
        state.Populate(5);

        var first = MacroActionCommand.Stop();
        var second = MacroActionCommand.Stop();

        Assert.That(second.TryMergeTo(first), Is.True);
        Assert.Multiple(() =>
        {
            Assert.That(first.Action, Is.EqualTo(MacroAction.Stop));
            Assert.That(first.Index, Is.EqualTo(0xFFFF));
        });
    }

    [Test]
    public void MergeStopRecord()
    {
        var state = new MacroSystem(Substitute.For<IAtemSwitcher>());
        state.Populate(5);

        var first = MacroActionCommand.StopRecord();
        var second = MacroActionCommand.StopRecord();

        Assert.That(second.TryMergeTo(first), Is.True);
        Assert.Multiple(() =>
        {
            Assert.That(first.Action, Is.EqualTo(MacroAction.StopRecord));
            Assert.That(first.Index, Is.EqualTo(0xFFFF));
        });
    }

    [Test]
    public void MergeInsertUserWait()
    {
        var state = new MacroSystem(Substitute.For<IAtemSwitcher>());
        state.Populate(5);

        var first = MacroActionCommand.InsertUserWait();
        var second = MacroActionCommand.InsertUserWait();

        Assert.That(second.TryMergeTo(first), Is.True);
        Assert.Multiple(() =>
        {
            Assert.That(first.Action, Is.EqualTo(MacroAction.InsertUserWait));
            Assert.That(first.Index, Is.EqualTo(0xFFFF));
        });
    }

    [Test]
    public void MergeContinue()
    {
        var state = new MacroSystem(Substitute.For<IAtemSwitcher>());
        state.Populate(5);

        var first = MacroActionCommand.Continue();
        var second = MacroActionCommand.Continue();

        Assert.That(second.TryMergeTo(first), Is.True);
        Assert.Multiple(() =>
        {
            Assert.That(first.Action, Is.EqualTo(MacroAction.Continue));
            Assert.That(first.Index, Is.EqualTo(0xFFFF));
        });
    }

    [Test]
    public void MergeDeleteIfIndexIsEqual()
    {
        var state = new MacroSystem(Substitute.For<IAtemSwitcher>());
        state.Populate(5);

        var first = MacroActionCommand.Delete(state[2]);
        var second = MacroActionCommand.Delete(state[2]);

        Assert.That(second.TryMergeTo(first), Is.True);
        Assert.Multiple(() =>
        {
            Assert.That(first.Action, Is.EqualTo(MacroAction.Delete));
            Assert.That(first.Index, Is.EqualTo(2));
        });
    }

    [Test]
    public void DontMergeDeleteIfIndexIsDifferent()
    {
        var state = new MacroSystem(Substitute.For<IAtemSwitcher>());
        state.Populate(5);

        var first = MacroActionCommand.Delete(state[2]);
        var second = MacroActionCommand.Delete(state[3]);

        Assert.That(second.TryMergeTo(first), Is.False);
        Assert.Multiple(() =>
        {
            Assert.That(first.Action, Is.EqualTo(MacroAction.Delete));
            Assert.That(first.Index, Is.EqualTo(2));
            Assert.That(second.Action, Is.EqualTo(MacroAction.Delete));
            Assert.That(second.Index, Is.EqualTo(3));
        });
    }

    [Test]
    public void DoesNotMergeWithDifferentCommandType()
    {
        var state = new MacroSystem(Substitute.For<IAtemSwitcher>());
        state.Populate(5);

        var sut = MacroActionCommand.InsertUserWait();

        Assert.That(sut.TryMergeTo(new MergeableCommand(2)), Is.False);
    }

    [Test]
    public void DoesNotMergeWithDifferentAction()
    {
        var state = new MacroSystem(Substitute.For<IAtemSwitcher>());
        state.Populate(5);

        var sut = MacroActionCommand.InsertUserWait();

        Assert.That(sut.TryMergeTo(MacroActionCommand.Continue()), Is.False);
    }
}
