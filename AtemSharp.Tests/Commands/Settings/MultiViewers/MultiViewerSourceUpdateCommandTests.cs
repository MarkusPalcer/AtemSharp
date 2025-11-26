using AtemSharp.Commands.Settings.MultiViewers;

namespace AtemSharp.Tests.Commands.Settings.MultiViewers;

[TestFixture]
public class MultiViewerSourceUpdateCommandTests : DeserializedCommandTestBase<MultiViewerSourceUpdateCommand,
    MultiViewerSourceUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int MultiviewIndex { get; set; } // Match TypeScript property name
        public int WindowIndex { get; set; }
        public int Source { get; set; }
        public bool SupportVuMeter { get; set; } // Match TypeScript property name (no 's')
        public bool SupportsSafeArea { get; set; }
    }

    protected override void CompareCommandProperties(MultiViewerSourceUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.MultiViewerId, Is.EqualTo(expectedData.MultiviewIndex));
        Assert.That(actualCommand.WindowIndex, Is.EqualTo(expectedData.WindowIndex));
        Assert.That(actualCommand.Source, Is.EqualTo(expectedData.Source));
        Assert.That(actualCommand.SupportsVuMeter, Is.EqualTo(expectedData.SupportVuMeter));
        Assert.That(actualCommand.SupportsSafeArea, Is.EqualTo(expectedData.SupportsSafeArea));
    }
}
