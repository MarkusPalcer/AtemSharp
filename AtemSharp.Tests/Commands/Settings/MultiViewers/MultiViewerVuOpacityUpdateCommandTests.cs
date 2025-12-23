using AtemSharp.Commands.Settings.MultiViewers;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Settings.MultiViewers;

[TestFixture]
internal class MultiViewerVuOpacityUpdateCommandTests : DeserializedCommandTestBase<MultiViewerVuOpacityUpdateCommand,
    MultiViewerVuOpacityUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MultiviewIndex { get; set; }
        public byte Opacity { get; set; }
    }

    internal override void CompareCommandProperties(MultiViewerVuOpacityUpdateCommand actualCommand, CommandData expectedData, TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        Assert.That(actualCommand.Opacity, Is.EqualTo(expectedData.Opacity));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Settings.MultiViewers.GetOrCreate(expectedData.MultiviewIndex);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.Settings.MultiViewers[expectedData.MultiviewIndex].VuOpacity, Is.EqualTo(expectedData.Opacity));
    }
}
