using AtemSharp.Commands.Macro;
using AtemSharp.State;
using AtemSharp.State.Macro;
using AtemSharp.State.Settings;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.State;

[TestFixture]
public class MacroSystemTests
{
    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void ChangeIsLooping(bool newValue)
    {
        var switcher = new AtemSwitcherFake();
        var sut = new MacroSystem(switcher);
        sut.Populate(5);

        sut.Player.UpdatePlayLooped(!newValue);
        sut.Player.PlayLooped = newValue;

        Assert.That(switcher.SentCommands, Has.Count.EqualTo(1));
        var command = switcher.SentCommands[0].As<MacroRunStatusCommand>();
        Assert.That(command.Loop, Is.EqualTo(newValue));
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void NoChangeIsLooping(bool newValue)
    {
        var switcher = new AtemSwitcherFake();
        var sut = new MacroSystem(switcher);
        sut.Populate(5);

        sut.Player.UpdatePlayLooped(newValue);
        sut.Player.PlayLooped = newValue;

        Assert.That(switcher.SentCommands, Has.Count.EqualTo(0));
    }

    [Test]
    public async Task RunMacro()
    {
        var switcher = new AtemSwitcherFake();

        var sut = new MacroSystem(switcher);
        sut.Populate(5);
        sut[2].UpdateIsUsed(true);
        await sut[2].Run().WithTimeout();

        Assert.That(switcher.SentCommands, Has.Count.EqualTo(1));

        Assert.That(switcher.SentCommands, Has.Count.EqualTo(1));
        var command = switcher.SentCommands[0].As<MacroActionCommand>();
        Assert.Multiple(() =>
        {
            Assert.That(command.Index, Is.EqualTo(2));
            Assert.That(command.Action, Is.EqualTo(MacroAction.Run));
        });
    }

    [Test]
    public void RunUnusedMacro()
    {
        var switcher = new AtemSwitcherFake();

        var sut = new MacroSystem(switcher);
        sut.Populate(5);
        sut[2].UpdateIsUsed(false);

        Assert.ThrowsAsync<InvalidOperationException>(() => sut[2].Run().WithTimeout());
        Assert.That(switcher.SentCommands, Has.Count.EqualTo(0));
    }

    [Test]
    public async Task StopMacro()
    {
        var switcher = new AtemSwitcherFake();
        var sut = new MacroSystem(switcher);
        sut.Populate(5);

        sut.Player.UpdateCurrentlyPlaying(sut[2]);

        await sut.Player.StopPlayback().WithTimeout();

        Assert.That(switcher.SentCommands, Has.Count.EqualTo(1));
        var command = switcher.SentCommands[0].As<MacroActionCommand>();
        Assert.That(command.Action, Is.EqualTo(MacroAction.Stop));
    }

    [Test]
    public async Task StopMacroRecord()
    {
        var switcher = new AtemSwitcherFake();
        var sut = new MacroSystem(switcher);
        sut.Populate(5);

        sut.Recorder.UpdateCurrentlyRecording(sut[2]);

        await sut.Recorder.StopRecording().WithTimeout();

        Assert.That(switcher.SentCommands, Has.Count.EqualTo(1));
        var command = switcher.SentCommands[0].As<MacroActionCommand>();
        Assert.That(command.Action, Is.EqualTo(MacroAction.StopRecord));
    }

    [Test]
    public async Task AddPauseInFrames()
    {
        var switcher = new AtemSwitcherFake();
        var sut = new MacroSystem(switcher);
        sut.Populate(5);
        sut.Recorder.UpdateCurrentlyRecording(sut[2]);

        await sut.Recorder.AddPause(12).WithTimeout();

        Assert.That(switcher.SentCommands, Has.Count.EqualTo(1));
        var command = switcher.SentCommands[0].As<MacroAddTimedPauseCommand>();
        Assert.That(command.Frames, Is.EqualTo(12));
    }

    [Test]
    public void AddPauseWhileNotRecording()
    {
        var switcher = new AtemSwitcherFake();
        var sut = new MacroSystem(switcher);
        sut.Populate(5);
        sut.Recorder.UpdateCurrentlyRecording(null);

        Assert.ThrowsAsync<InvalidOperationException>(() => sut.Recorder.AddPause(12).WithTimeout());
        Assert.That(switcher.SentCommands, Has.Count.EqualTo(0));
    }

    [Test]
    [TestCase(1, (ushort)24)]
    [TestCase(0.5, (ushort)12)]
    public async Task AddPauseInTimeSpan(double durationInSeconds, ushort expectedFrames)
    {
        var switcher = new AtemSwitcherFake();
        switcher.State = new AtemState
        {
            Settings =
            {
                VideoMode = VideoMode.N4KHDp24
            }
        };

        var sut = new MacroSystem(switcher);
        sut.Populate(5);
        sut.Recorder.UpdateCurrentlyRecording(sut[2]);

        await sut.Recorder.AddPause(TimeSpan.FromSeconds(durationInSeconds)).WithTimeout();

        Assert.That(switcher.SentCommands, Has.Count.EqualTo(1));
        var command = switcher.SentCommands[0].As<MacroAddTimedPauseCommand>();
        Assert.That(command.Frames, Is.EqualTo(expectedFrames));
    }

    [Test]
    public void ChangeMacroName()
    {
        var switcher = new AtemSwitcherFake();
        var sut = new MacroSystem(switcher);
        sut.Populate(5);

        sut[2].Name = "New Name";

        Assert.That(switcher.SentCommands, Has.Count.EqualTo(1));
        var command = switcher.SentCommands[0].As<MacroPropertiesCommand>();
        Assert.That(command.Name, Is.EqualTo("New Name"));
    }

    [Test]
    public void ChangeMacroDescription()
    {
        var switcher = new AtemSwitcherFake();
        var sut = new MacroSystem(switcher);
        sut.Populate(5);

        sut[2].Description = "New Description";

        Assert.That(switcher.SentCommands, Has.Count.EqualTo(1));
        var command = switcher.SentCommands[0].As<MacroPropertiesCommand>();
        Assert.That(command.Description, Is.EqualTo("New Description"));
    }

    [Test]
    public async Task RecordMacro()
    {
        var switcher = new AtemSwitcherFake();
        var sut = new MacroSystem(switcher);
        sut.Populate(5);

        await sut[2].Record("My Macro", "My Description").WithTimeout();

        Assert.That(switcher.SentCommands, Has.Count.EqualTo(1));
        var command = switcher.SentCommands[0].As<MacroRecordCommand>();
        Assert.Multiple(() =>
        {
            Assert.That(command.Index, Is.EqualTo(2));
            Assert.That(command.Name, Is.EqualTo("My Macro"));
            Assert.That(command.Description, Is.EqualTo("My Description"));
        });
    }

    [Test]
    public async Task Continue()
    {
        var switcher = new AtemSwitcherFake();
        var sut = new MacroSystem(switcher);
        sut.Populate(5);

        await sut.Player.Continue().WithTimeout();

        Assert.That(switcher.SentCommands, Has.Count.EqualTo(1));
        var command = switcher.SentCommands[0].As<MacroActionCommand>();
        Assert.That(command.Action, Is.EqualTo(MacroAction.Continue));
    }

    [Test]
    public async Task AddWaitForUser()
    {
        var switcher = new AtemSwitcherFake();
        var sut = new MacroSystem(switcher);

        sut.Populate(5);
        sut.Recorder.UpdateCurrentlyRecording(sut[2]);

        await sut.Recorder.AddWaitForUser().WithTimeout();

        Assert.That(switcher.SentCommands, Has.Count.EqualTo(1));
        var command = switcher.SentCommands[0].As<MacroActionCommand>();
        Assert.That(command.Action, Is.EqualTo(MacroAction.InsertUserWait));
    }

    [Test]
    public void AddWaitForUser_WithoutRecording()
    {
        var switcher = new AtemSwitcherFake();
        var sut = new MacroSystem(switcher);

        Assert.ThrowsAsync<InvalidOperationException>(() => sut.Recorder.AddWaitForUser().WithTimeout());
    }
}
