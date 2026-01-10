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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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

        await sut.SendCommandAsync(new NonMergeableCommand(2));
        await sut.SendCommandAsync(new NonMergeableCommand(4));
        await sut.SendCommandAsync(new NonMergeableCommand(6));
        await sut.SendCommandAsync(new NonMergeableCommand(8));

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
}
