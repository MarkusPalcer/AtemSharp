using AtemSharp.Commands.Settings.MultiViewers;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Settings.MultiViewers;

[TestFixture]
public class MultiViewerWindowVuMeterUpdateCommandTests : DeserializedCommandTestBase<MultiViewerWindowVuMeterUpdateCommand,
    MultiViewerWindowVuMeterUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int MultiviewIndex { get; set; } // Match TypeScript property name
        public int WindowIndex { get; set; }
        public bool VuEnabled { get; set; }
    }

    protected override void CompareCommandProperties(MultiViewerWindowVuMeterUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.MultiViewerId, Is.EqualTo(expectedData.MultiviewIndex),
                    $"MultiViewerId should match expected value for test case {testCase.Name}");
        Assert.That(actualCommand.WindowIndex, Is.EqualTo(expectedData.WindowIndex),
                    $"WindowIndex should match expected value for test case {testCase.Name}");
        Assert.That(actualCommand.VuEnabled, Is.EqualTo(expectedData.VuEnabled),
                    $"VuEnabled should match expected value for test case {testCase.Name}");
    }

    [Test]
    public void ApplyToState_WithValidMultiViewer_UpdatesWindow()
    {
        // Arrange
        const int multiViewerId = 1;
        const int windowIndex = 5;
        const bool vuEnabled = true;

        var state = CreateStateWithMultiViewer(multiViewerId);
        var command = new MultiViewerWindowVuMeterUpdateCommand
        {
            MultiViewerId = multiViewerId,
            WindowIndex = windowIndex,
            VuEnabled = vuEnabled
        };

        // Act
        command.ApplyToState(state);

        // Assert
        var multiViewer = state.Settings.MultiViewers[multiViewerId];
        Assert.That(multiViewer.Windows.ContainsKey(windowIndex), Is.True, "Window should exist");

        var window = multiViewer.Windows[windowIndex];
        Assert.That(window.AudioMeter, Is.EqualTo(vuEnabled));
    }

    [Test]
    public void ApplyToState_WithExistingWindow_UpdatesAudioMeter()
    {
        // Arrange
        const int multiViewerId = 0;
        const int windowIndex = 2;

        var state = CreateStateWithMultiViewer(multiViewerId);

        // Pre-populate window with different AudioMeter value
        var multiViewer = state.Settings.MultiViewers[multiViewerId];
        multiViewer.Windows[windowIndex] = new MultiViewerWindowState
        {
            AudioMeter = false,
            WindowIndex = windowIndex
        };

        var command = new MultiViewerWindowVuMeterUpdateCommand
        {
            MultiViewerId = multiViewerId,
            WindowIndex = windowIndex,
            VuEnabled = true
        };

        // Act
        command.ApplyToState(state);

        // Assert
        var window = multiViewer.Windows[windowIndex];
        Assert.That(window.AudioMeter, Is.True, "AudioMeter should be updated to true");
    }

    [Test]
    public void ApplyToState_WithNewWindow_CreatesWindowWithAudioMeter()
    {
        // Arrange
        const int multiViewerId = 0;
        const int windowIndex = 7;

        var state = CreateStateWithMultiViewer(multiViewerId);
        var command = new MultiViewerWindowVuMeterUpdateCommand
        {
            MultiViewerId = multiViewerId,
            WindowIndex = windowIndex,
            VuEnabled = false
        };

        // Act
        command.ApplyToState(state);

        // Assert
        var multiViewer = state.Settings.MultiViewers[multiViewerId];
        Assert.That(multiViewer.Windows.ContainsKey(windowIndex), Is.True, "New window should be created");

        var window = multiViewer.Windows[windowIndex];
        Assert.That(window.AudioMeter, Is.False, "AudioMeter should be set to false");
    }

    /// <summary>
    /// Creates an AtemState with a valid MultiViewer at the specified index
    /// </summary>
    private static AtemState CreateStateWithMultiViewer(byte multiViewerId)
    {
        return new AtemState
        {
            Settings = new SettingsState
            {
                MultiViewers = AtemStateUtil.CreateArray<MultiViewer>(multiViewerId + 1)
            },
        };
    }
}
