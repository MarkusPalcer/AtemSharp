using AtemSharp.Commands.Settings.MultiViewers;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Settings.MultiViewers;

[TestFixture]
public class MultiViewerWindowVuMeterCommandTests : SerializedCommandTestBase<MultiViewerWindowVuMeterCommand,
    MultiViewerWindowVuMeterCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MultiviewIndex { get; set; }
        public byte WindowIndex { get; set; }
        public bool VuEnabled { get; set; }
    }

    protected override MultiViewerWindowVuMeterCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required MultiViewer
        var state = new MultiViewerWindowState()
        {
            MultiViewerId = testCase.Command.MultiviewIndex,
            WindowIndex = testCase.Command.WindowIndex,
            AudioMeter = testCase.Command.VuEnabled
        };

        // Create command with the MultiViewer ID
        return new MultiViewerWindowVuMeterCommand(state);
    }

    [Test]
    public void Constructor_WithValidState_InitializesWithDefaults()
    {
        // Arrange
        var state = new MultiViewerWindowState()
        {
            MultiViewerId = 1,
            WindowIndex = 2,
            AudioMeter = true
        };

        // Act
        var command = new MultiViewerWindowVuMeterCommand(state);

        // Assert
        Assert.That(command.MultiViewerId, Is.EqualTo(1));
        Assert.That(command.WindowIndex, Is.EqualTo(2));
        Assert.That(command.VuEnabled, Is.True);
    }
}
