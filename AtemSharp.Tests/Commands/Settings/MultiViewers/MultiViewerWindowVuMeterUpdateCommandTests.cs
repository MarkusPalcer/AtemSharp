using AtemSharp.Commands.Settings.MultiViewers;

namespace AtemSharp.Tests.Commands.Settings.MultiViewers;

[TestFixture]
public class MultiViewerWindowVuMeterUpdateCommandTests : DeserializedCommandTestBase<MultiViewerWindowVuMeterUpdateCommand,
    MultiViewerWindowVuMeterUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int MultiviewIndex { get; set; }
        public int WindowIndex { get; set; }
        public bool VuEnabled { get; set; }
    }

    protected override void CompareCommandProperties(MultiViewerWindowVuMeterUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.MultiViewerId, Is.EqualTo(expectedData.MultiviewIndex));
        Assert.That(actualCommand.WindowIndex, Is.EqualTo(expectedData.WindowIndex));
        Assert.That(actualCommand.VuEnabled, Is.EqualTo(expectedData.VuEnabled));
    }

}
