using AtemSharp.Commands.DownstreamKey;

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
}
