using AtemSharp.Commands.Settings.MultiViewers;
using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Tests.Commands.Settings.MultiViewers;

[TestFixture]
public class MultiViewerWindowVuMeterCommandTests : SerializedCommandTestBase<MultiViewerWindowVuMeterCommand,
    MultiViewerWindowVuMeterCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int MultiviewIndex { get; set; } // Match TypeScript property name
        public int WindowIndex { get; set; }
        public bool VuEnabled { get; set; }
    }

    protected override MultiViewerWindowVuMeterCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required MultiViewer
        var state = CreateStateWithMultiViewer(testCase.Command.MultiviewIndex);

        // Create command with the MultiViewer ID
        var command = new MultiViewerWindowVuMeterCommand(testCase.Command.MultiviewIndex, state);

        // Set the actual values that should be written
        command.WindowIndex = testCase.Command.WindowIndex;
        command.VuEnabled = testCase.Command.VuEnabled;

        return command;
    }

    /// <summary>
    /// Creates an AtemState with a valid MultiViewer at the specified index
    /// </summary>
    private static AtemState CreateStateWithMultiViewer(int multiViewerId)
    {
        var multiViewers = new Dictionary<int, MultiViewer>
        {
            { multiViewerId, new MultiViewer(multiViewerId) }
        };

        return new AtemState
        {
            Settings = new SettingsState
            {
                MultiViewers = multiViewers
            },
            Info = new DeviceInfo
            {
                MultiViewer = new MultiViewerInfo
                {
                    Count = multiViewerId + 1,
                    WindowCount = 10
                }
            }
        };
    }

    [Test]
    public void Constructor_WithEmptyState_ThrowsInvalidIdError()
    {
        // Arrange
        const int multiViewerId = 1;
        var state = new AtemState(); // Empty state

        // Act & Assert
        Assert.Throws<InvalidIdError>(() => _ = new MultiViewerWindowVuMeterCommand(multiViewerId, state));
    }

    [Test]
    public void Constructor_WithValidState_InitializesWithDefaults()
    {
        // Arrange
        const int multiViewerId = 1;
        var state = CreateStateWithMultiViewer(multiViewerId);

        // Act
        var command = new MultiViewerWindowVuMeterCommand(multiViewerId, state);

        // Assert
        Assert.That(command.MultiViewerId, Is.EqualTo(multiViewerId));
        Assert.That(command.WindowIndex, Is.EqualTo(0));
        Assert.That(command.VuEnabled, Is.False);
    }

    [Test]
    public void ConvenienceConstructor_SetsPropertiesCorrectly()
    {
        // Arrange
        const int multiViewerId = 1;
        const int windowIndex = 5;
        const bool vuEnabled = true;
        var state = CreateStateWithMultiViewer(multiViewerId);

        // Act
        var command = new MultiViewerWindowVuMeterCommand(multiViewerId, windowIndex, vuEnabled, state);

        // Assert
        Assert.That(command.MultiViewerId, Is.EqualTo(multiViewerId));
        Assert.That(command.WindowIndex, Is.EqualTo(windowIndex));
        Assert.That(command.VuEnabled, Is.EqualTo(vuEnabled));
    }

    [Test]
    public void Constructor_WithInvalidMultiViewerId_ThrowsInvalidIdError()
    {
        // Arrange
        const int invalidMultiViewerId = 5;
        var state = CreateStateWithMultiViewer(0); // Only MultiViewer 0 exists

        // Act & Assert
        Assert.Throws<InvalidIdError>(() => _ = new MultiViewerWindowVuMeterCommand(invalidMultiViewerId, state));
    }

    [Test]
    public void Properties_CanBeSetAndRetrieved()
    {
        // Arrange
        const int multiViewerId = 0;
        var state = CreateStateWithMultiViewer(multiViewerId);
        var command = new MultiViewerWindowVuMeterCommand(multiViewerId, state);

        // Act & Assert - WindowIndex
        command.WindowIndex = 3;
        Assert.That(command.WindowIndex, Is.EqualTo(3));

        // Act & Assert - VuEnabled
        command.VuEnabled = true;
        Assert.That(command.VuEnabled, Is.True);
    }
}