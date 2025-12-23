using AtemSharp.Commands.DownstreamKey;
using AtemSharp.State;
using AtemSharp.State.Info;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Commands.DownstreamKey;

internal class DownstreamKeyStateUpdateCommandTests : DeserializedCommandTestBase<DownstreamKeyStateUpdateCommand,
    DownstreamKeyStateUpdateCommandTests.CommandData>
{
    [MaxProtocolVersion(ProtocolVersion.V8_0)]
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public bool OnAir { get; set; }
        public bool InTransition { get; set; }
        public bool IsAuto { get; set; }
        public int RemainingFrames { get; set; }
    }

    internal override void CompareCommandProperties(DownstreamKeyStateUpdateCommand actualCommand, CommandData expectedData, TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        Assert.That(actualCommand.Index, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.OnAir, Is.EqualTo(expectedData.OnAir));
        Assert.That(actualCommand.InTransition, Is.EqualTo(expectedData.InTransition));
        Assert.That(actualCommand.IsAuto, Is.EqualTo(expectedData.IsAuto));
        Assert.That(actualCommand.RemainingFrames, Is.EqualTo(expectedData.RemainingFrames));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Video.DownstreamKeyers.GetOrCreate(expectedData.Index);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var target = state.Video.DownstreamKeyers[expectedData.Index];
        Assert.That(target.Id, Is.EqualTo(expectedData.Index));
        Assert.That(target.OnAir, Is.EqualTo(expectedData.OnAir));
        Assert.That(target.InTransition, Is.EqualTo(expectedData.InTransition));
        Assert.That(target.IsAuto, Is.EqualTo(expectedData.IsAuto));
        Assert.That(target.RemainingFrames, Is.EqualTo(expectedData.RemainingFrames));
    }
}
