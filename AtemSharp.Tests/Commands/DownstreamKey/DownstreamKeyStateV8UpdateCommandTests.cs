using AtemSharp.Commands.DownstreamKey;
using AtemSharp.State;
using AtemSharp.State.Video.DownstreamKeyer;

namespace AtemSharp.Tests.Commands.DownstreamKey;

public class DownstreamKeyStateV8UpdateCommandTests : DeserializedCommandTestBase<DownstreamKeyStateV8UpdateCommand,
    DownstreamKeyStateV8UpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Index { get; set; }
        public bool OnAir { get; set; }
        public bool InTransition { get; set; }
        public bool IsAuto { get; set; }
        public int RemainingFrames { get; set; }

        public bool IsTowardsOnAir { get; set; }
    }

    protected override void CompareCommandProperties(DownstreamKeyStateV8UpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.Index, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.OnAir, Is.EqualTo(expectedData.OnAir));
        Assert.That(actualCommand.InTransition, Is.EqualTo(expectedData.InTransition));
        Assert.That(actualCommand.IsAuto, Is.EqualTo(expectedData.IsAuto));
        Assert.That(actualCommand.RemainingFrames, Is.EqualTo(expectedData.RemainingFrames));
        Assert.That(actualCommand.IsTowardsOnAir, Is.EqualTo(expectedData.IsTowardsOnAir));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Video.DownstreamKeyers = AtemStateUtil.CreateArray<DownstreamKeyer>(expectedData.Index + 1);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var target = state.Video.DownstreamKeyers[expectedData.Index];
        Assert.That(target.OnAir, Is.EqualTo(expectedData.OnAir));
        Assert.That(target.InTransition, Is.EqualTo(expectedData.InTransition));
        Assert.That(target.IsAuto, Is.EqualTo(expectedData.IsAuto));
        Assert.That(target.RemainingFrames, Is.EqualTo(expectedData.RemainingFrames));
        Assert.That(target.IsTowardsOnAir, Is.EqualTo(expectedData.IsTowardsOnAir));
    }
}
