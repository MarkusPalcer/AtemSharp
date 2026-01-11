using AtemSharp.Batch;
using AtemSharp.Commands;
using AtemSharp.State.Info;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Batch;

[TestFixture]
public class BatchOperationTests
{
    private class MergeableCommand(int value) : SerializedCommand
    {
        public List<int> Values { get; } = [value];

        public override byte[] Serialize(ProtocolVersion version)
        {
            return [];
        }

        internal override bool TryMergeTo(SerializedCommand other)
        {
            other.As<MergeableCommand>().Values.Add(value);
            return true;
        }
    }

    private class NonMergeableCommand(int value) : SerializedCommand
    {
        public int Value { get; } = value;

        public override byte[] Serialize(ProtocolVersion version)
        {
            return [];
        }

        internal override bool TryMergeTo(SerializedCommand other)
        {
            return false;
        }
    }

    [Test]
    public async Task QueueDifferentCommands()
    {
        var switcher = new AtemSwitcherFake();
        var sut = new BatchOperation(switcher);

        await sut.SendCommandsAsync([
            new NonMergeableCommand(2),
            new NonMergeableCommand(4),
            new NonMergeableCommand(6),
            new NonMergeableCommand(8),
        ]);

        Assert.That(switcher.SendRequests, Has.Count.EqualTo(0));

        await sut.CommitAsync();

        Assert.That(switcher.SendRequests, Has.Count.EqualTo(1));
        Assert.That(switcher.SentCommands, Has.Length.EqualTo(4));

        Assert.That(switcher.SentCommands.Select(x => x.As<NonMergeableCommand>().Value), Is.EquivalentTo(new[] { 2, 4, 6, 8 }));
    }

    [Test]
    public async Task MergeCommandsIfPossible()
    {
        var switcher = new AtemSwitcherFake();
        var sut = new BatchOperation(switcher);

        await sut.SendCommandAsync(new MergeableCommand(2));
        await sut.SendCommandAsync(new MergeableCommand(4));
        await sut.SendCommandAsync(new MergeableCommand(6));
        await sut.SendCommandAsync(new MergeableCommand(8));

        Assert.That(switcher.SendRequests, Has.Count.EqualTo(0));

        await sut.CommitAsync();

        Assert.That(switcher.SendRequests, Has.Count.EqualTo(1));
        Assert.That(switcher.SentCommands, Has.Length.EqualTo(1));

        var sentCommand = switcher.SentCommands[0].As<MergeableCommand>();
        Assert.That(sentCommand.Values, Is.EquivalentTo(new[] { 2, 4, 6, 8 }));
    }

    [Test]
    public async Task Dispose_CommitsQueuedCommands()
    {
        var switcher = new AtemSwitcherFake();
        var sut = new BatchOperation(switcher);

        await sut.SendCommandAsync(new MergeableCommand(2));

        await sut.DisposeAsync();

        Assert.That(switcher.SendRequests, Has.Count.EqualTo(1));
        Assert.That(switcher.SentCommands, Has.Length.EqualTo(1));
        var sentCommand = switcher.SentCommands[0].As<MergeableCommand>();
        Assert.That(sentCommand.Values, Is.EquivalentTo(new[] { 2 }));
    }

    [Test]
    public async Task Creation_LoadsFromSwitcher()
    {
        var switcher = new AtemSwitcherFake();
        switcher.Macros.Populate(5);
        for (ushort i = 0; i < 5; i++)
        {
            switcher.Macros[i].UpdateName($"Name {i}");
            switcher.Macros[i].UpdateDescription($"Description {i}");
            switcher.Macros[i].UpdateIsUsed(true);
            switcher.Macros[i].UpdateHasUnsupportedOps(i % 2 == 0);
        }
        switcher.Macros.Player.UpdateCurrentlyPlaying(switcher.Macros[0]);
        switcher.Macros.Player.UpdatePlayLooped(true);
        switcher.Macros.Player.UpdatePlaybackIsWaitingForUserAction(true);

        switcher.Macros.Recorder.UpdateCurrentlyRecording(switcher.Macros[1]);


        var sut = new BatchOperation(switcher);

        await Verify(sut);

        Assert.That(switcher.Macros.Player.CurrentlyPlaying!.Id, Is.EqualTo(sut.Macros.Player.CurrentlyPlaying!.Id));
        Assert.That(switcher.Macros.Player.CurrentlyPlaying.Name, Is.EqualTo(sut.Macros.Player.CurrentlyPlaying.Name));
        Assert.That(switcher.Macros.Player.CurrentlyPlaying.Description, Is.EqualTo(sut.Macros.Player.CurrentlyPlaying.Description));
        Assert.That(switcher.Macros.Player.CurrentlyPlaying.IsUsed, Is.EqualTo(sut.Macros.Player.CurrentlyPlaying.IsUsed));
        Assert.That(switcher.Macros.Player.CurrentlyPlaying.HasUnsupportedOps, Is.EqualTo(sut.Macros.Player.CurrentlyPlaying.HasUnsupportedOps));
        Assert.That(switcher.Macros.Player.PlaybackIsWaitingForUserAction, Is.EqualTo(sut.Macros.Player.PlaybackIsWaitingForUserAction));
        Assert.That(switcher.Macros.Player.PlayLooped, Is.EqualTo(sut.Macros.Player.PlayLooped));

        Assert.That(switcher.Macros.Recorder.CurrentlyRecording!.Id, Is.EqualTo(sut.Macros.Recorder.CurrentlyRecording!.Id));
        Assert.That(switcher.Macros.Recorder.CurrentlyRecording.Name, Is.EqualTo(sut.Macros.Recorder.CurrentlyRecording.Name));
        Assert.That(switcher.Macros.Recorder.CurrentlyRecording.Description, Is.EqualTo(sut.Macros.Recorder.CurrentlyRecording.Description));
        Assert.That(switcher.Macros.Recorder.CurrentlyRecording.IsUsed, Is.EqualTo(sut.Macros.Recorder.CurrentlyRecording.IsUsed));
        Assert.That(switcher.Macros.Recorder.CurrentlyRecording.HasUnsupportedOps, Is.EqualTo(sut.Macros.Recorder.CurrentlyRecording.HasUnsupportedOps));
    }

    [Test]
    public void Revert_ReloadsFromSwitcher()
    {
        var switcher = new AtemSwitcherFake();
        switcher.Macros.Populate(5);
        for (ushort i = 0; i < 5; i++)
        {
            switcher.Macros[i].UpdateName($"Name {i}");
            switcher.Macros[i].UpdateDescription($"Description {i}");
        }

        var sut = new BatchOperation(switcher);

        switcher.Macros[1].UpdateName("New Name");

        Assert.That(sut.Macros[1].Name, Is.EqualTo("Name 1"));

        sut.Revert();
        Assert.That(sut.Macros[1].Name, Is.EqualTo("New Name"));
    }

    [Test]
    public async Task Revert_ClearsQueue()
    {
        var switcher = new AtemSwitcherFake();
        var sut =  new BatchOperation(switcher);

        await sut.SendCommandAsync(new MergeableCommand(2));

        sut.Revert();
        await sut.CommitAsync();

        Assert.That(switcher.SendRequests, Has.Count.EqualTo(0));
    }

    [Test]
    public async Task Commit_ClearsQueue()
    {
        var switcher = new AtemSwitcherFake();
        var sut =  new BatchOperation(switcher);

        await sut.SendCommandAsync(new MergeableCommand(2));
        await sut.CommitAsync();
        await sut.CommitAsync();

        Assert.That(switcher.SendRequests, Has.Count.EqualTo(1));
        Assert.That(switcher.SentCommands, Has.Length.EqualTo(1));
        Assert.That(switcher.SentCommands[0].As<MergeableCommand>().Values, Is.EquivalentTo(new[] { 2 }));
    }

    [Test]
    public async Task EmptyCommit_DoesNotSend()
    {
        var switcher = new AtemSwitcherFake();
        var sut =  new BatchOperation(switcher);

        await sut.CommitAsync();
        Assert.That(switcher.SendRequests, Has.Count.EqualTo(0));
    }
}
