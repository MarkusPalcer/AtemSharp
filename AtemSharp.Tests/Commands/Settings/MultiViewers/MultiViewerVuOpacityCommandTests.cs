using AtemSharp.Commands.Settings.MultiViewers;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Settings.MultiViewers;

[TestFixture]
public class MultiViewerVuOpacityCommandTests : SerializedCommandTestBase<MultiViewerVuOpacityCommand,
    MultiViewerVuOpacityCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MultiviewIndex { get; set; }
        public byte Opacity { get; set; }
    }

    protected override MultiViewerVuOpacityCommand CreateSut(TestCaseData testCase)
    {
        var state = new MultiViewer
        {
            Index = testCase.Command.MultiviewIndex,
            VuOpacity = testCase.Command.Opacity
        };

        return new MultiViewerVuOpacityCommand(state);
    }

    [Test]
    public void Constructor_InitializesFromState()
    {
        // Arrange
        const int multiViewerId = 1;
        var state = new MultiViewer
        {
            Index = multiViewerId,
            VuOpacity = 50
        };

        // Act
        var command = new MultiViewerVuOpacityCommand(state);

        // Assert
        Assert.That(command.MultiViewerId, Is.EqualTo(multiViewerId));
        Assert.That(command.Opacity, Is.EqualTo(50));
    }
}
