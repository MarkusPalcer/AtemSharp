using AtemSharp.Commands.DownstreamKey;
using AtemSharp.State;
using AtemSharp.State.Video.DownstreamKeyer;

namespace AtemSharp.Tests.Commands.DownstreamKey;

public class DownstreamKeySourcesUpdateCommandTests : DeserializedCommandTestBase<DownstreamKeySourcesUpdateCommand,
    DownstreamKeySourcesUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Index { get; set; }

        public int CutSource { get; set; }

        public int FillSource { get; set; }
    }

    protected override void CompareCommandProperties(DownstreamKeySourcesUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.DownstreamKeyerId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.CutSource, Is.EqualTo(expectedData.CutSource));
        Assert.That(actualCommand.FillSource, Is.EqualTo(expectedData.FillSource));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Video.DownstreamKeyers = AtemStateUtil.CreateArray<DownstreamKeyer>(expectedData.Index + 1);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var target = state.Video.DownstreamKeyers[expectedData.Index].Sources;
        Assert.That(target.CutSource, Is.EqualTo(expectedData.CutSource));
        Assert.That(target.FillSource, Is.EqualTo(expectedData.FillSource));
    }
}
