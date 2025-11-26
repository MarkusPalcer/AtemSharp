using AtemSharp.Commands.Settings.MultiViewers;

namespace AtemSharp.Tests.Commands.Settings.MultiViewers;

[TestFixture]
public class MultiViewerWindowSafeAreaUpdateCommandTests : DeserializedCommandTestBase<MultiViewerWindowSafeAreaUpdateCommand,
    MultiViewerWindowSafeAreaUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MultiviewIndex { get; set; }
        public int WindowIndex { get; set; }
        public bool SafeAreaEnabled { get; set; }
    }

    protected override void CompareCommandProperties(MultiViewerWindowSafeAreaUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.MultiViewerId, Is.EqualTo(expectedData.MultiviewIndex));
        Assert.That(actualCommand.WindowIndex, Is.EqualTo(expectedData.WindowIndex));
        Assert.That(actualCommand.SafeAreaEnabled, Is.EqualTo(expectedData.SafeAreaEnabled));
    }
}
