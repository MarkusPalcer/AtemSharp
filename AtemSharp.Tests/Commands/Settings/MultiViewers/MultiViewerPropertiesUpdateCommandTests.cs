using AtemSharp.Commands.Settings.MultiViewers;
using AtemSharp.State;
using AtemSharp.State.Settings.MultiViewer;

namespace AtemSharp.Tests.Commands.Settings.MultiViewers;

[TestFixture]
internal class MultiViewerPropertiesUpdateCommandTests : DeserializedCommandTestBase<MultiViewerPropertiesUpdateCommand,
    MultiViewerPropertiesUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MultiviewIndex { get; set; }
        public MultiViewerLayout Layout { get; set; }
        public bool ProgramPreviewSwapped { get; set; }
    }

    internal override void CompareCommandProperties(MultiViewerPropertiesUpdateCommand actualCommand, CommandData expectedData, TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        Assert.That(actualCommand.MultiViewerId, Is.EqualTo(expectedData.MultiviewIndex));
        Assert.That(actualCommand.Layout, Is.EqualTo(expectedData.Layout));
        Assert.That(actualCommand.ProgramPreviewSwapped, Is.EqualTo(expectedData.ProgramPreviewSwapped));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var actualCommand = state.Settings.MultiViewers[expectedData.MultiviewIndex];
        Assert.That(actualCommand.Id, Is.EqualTo(expectedData.MultiviewIndex));
        Assert.That(actualCommand.Properties.Layout, Is.EqualTo(expectedData.Layout));
        Assert.That(actualCommand.Properties.ProgramPreviewSwapped, Is.EqualTo(expectedData.ProgramPreviewSwapped));
    }
}
