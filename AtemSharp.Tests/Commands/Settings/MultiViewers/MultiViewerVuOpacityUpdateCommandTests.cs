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

    [Test]
    public void ApplyToState_UpdatesVuOpacity()
    {
        // Arrange
        const int multiViewerId = 1;
        const int opacity = 85;

        var state = new AtemState
        {
            Settings =
            {
                MultiViewers = AtemStateUtil.CreateArray<MultiViewer>(multiViewerId + 1)
            }
        };

        var command = new MultiViewerVuOpacityUpdateCommand
        {
            MultiViewerId = multiViewerId,
            Opacity = opacity
        };

        // Act
        command.ApplyToState(state);

        // Assert
        var multiViewer = state.Settings.MultiViewers[multiViewerId];
        Assert.That(multiViewer.VuOpacity, Is.EqualTo(opacity));
    }

    protected override void CompareCommandProperties(MultiViewerVuOpacityUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.Opacity, Is.EqualTo(expectedData.Opacity));
    }
}
