using AtemSharp.Commands.Settings.MultiViewers;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Settings.MultiViewers;

[TestFixture]
public class MultiViewerVuOpacityUpdateCommandTests : DeserializedCommandTestBase<MultiViewerVuOpacityUpdateCommand,
    MultiViewerVuOpacityUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MultiviewIndex { get; set; }
        public byte Opacity { get; set; }
    }

    protected override void CompareCommandProperties(MultiViewerVuOpacityUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.Opacity, Is.EqualTo(expectedData.Opacity));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Settings.MultiViewers.ExpandToFit(expectedData.MultiviewIndex);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.Settings.MultiViewers[expectedData.MultiviewIndex].VuOpacity, Is.EqualTo(expectedData.Opacity));
    }
}
