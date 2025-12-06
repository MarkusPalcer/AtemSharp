using AtemSharp.Commands.Settings.MultiViewers;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Settings.MultiViewers;

[TestFixture]
public class MultiViewerWindowSafeAreaUpdateCommandTests : DeserializedCommandTestBase<MultiViewerWindowSafeAreaUpdateCommand,
    MultiViewerWindowSafeAreaUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MultiviewIndex { get; set; }
        public byte WindowIndex { get; set; }
        public bool SafeAreaEnabled { get; set; }
    }

    protected override void CompareCommandProperties(MultiViewerWindowSafeAreaUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.MultiViewerId, Is.EqualTo(expectedData.MultiviewIndex));
        Assert.That(actualCommand.WindowIndex, Is.EqualTo(expectedData.WindowIndex));
        Assert.That(actualCommand.SafeAreaEnabled, Is.EqualTo(expectedData.SafeAreaEnabled));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Settings.MultiViewers.ExpandToFit(expectedData.MultiviewIndex);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var window = state.Settings.MultiViewers[expectedData.MultiviewIndex].Windows[expectedData.WindowIndex];
        Assert.That(window.MultiViewerId, Is.EqualTo(expectedData.MultiviewIndex));
        Assert.That(window.WindowIndex, Is.EqualTo(expectedData.WindowIndex));
        Assert.That(window.SafeTitle, Is.EqualTo(expectedData.SafeAreaEnabled));
    }
}
