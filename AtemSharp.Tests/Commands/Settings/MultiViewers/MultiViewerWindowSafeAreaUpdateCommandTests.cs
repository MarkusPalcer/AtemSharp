using AtemSharp.Commands.Settings.MultiViewers;
using AtemSharp.State;
using AtemSharp.State.Info;

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


    private static AtemState CreateStateWithMultiViewer(byte multiViewerId)
    {
        var multiViewers = new Dictionary<int, MultiViewer>
        {
            { multiViewerId, new MultiViewer { Index = multiViewerId} }
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
    public void ApplyToState_WithValidMultiViewer_UpdatesWindow()
    {
        // Arrange
        const int multiViewerId = 1;
        const int windowIndex = 5;
        const bool safeAreaEnabled = true;

        var state = CreateStateWithMultiViewer(multiViewerId);
        var command = new MultiViewerWindowSafeAreaUpdateCommand
        {
            MultiViewerId = multiViewerId,
            WindowIndex = windowIndex,
            SafeAreaEnabled = safeAreaEnabled
        };

        // Act
        command.ApplyToState(state);

        // Assert
        var multiViewer = AtemStateUtil.GetMultiViewer(state, multiViewerId);
        Assert.That(multiViewer.Windows.ContainsKey(windowIndex), Is.True, "Window should exist");

        var window = multiViewer.Windows[windowIndex];
        Assert.That(window.SafeTitle, Is.EqualTo(safeAreaEnabled), "SafeTitle should be updated");
    }

    [Test]
    public void ApplyToState_WithExistingWindow_PreservesOtherProperties()
    {
        // Arrange
        const int multiViewerId = 1;
        const int windowIndex = 3;
        const bool safeAreaEnabled = false;

        var state = CreateStateWithMultiViewer(multiViewerId);
        var multiViewer = AtemStateUtil.GetMultiViewer(state, multiViewerId);

        // Pre-populate window with existing properties
        multiViewer.Windows[windowIndex] = new MultiViewerWindowState
        {
            SafeTitle = true,
            AudioMeter = true
        };

        var command = new MultiViewerWindowSafeAreaUpdateCommand
        {
            MultiViewerId = multiViewerId,
            WindowIndex = windowIndex,
            SafeAreaEnabled = safeAreaEnabled
        };

        // Act
        command.ApplyToState(state);

        // Assert
        var window = multiViewer.Windows[windowIndex];
        Assert.That(window.SafeTitle, Is.EqualTo(safeAreaEnabled), "SafeTitle should be updated");
        Assert.That(window.AudioMeter, Is.True, "AudioMeter should be preserved");
    }

    [Test]
    public void ApplyToState_WithInvalidMultiViewer_ThrowsInvalidIdError()
    {
        // Arrange
        const int multiViewerId = 5; // Not in state
        var state = CreateStateWithMultiViewer(1); // Only has MultiViewer 1
        var command = new MultiViewerWindowSafeAreaUpdateCommand
        {
            MultiViewerId = multiViewerId,
            WindowIndex = 0,
            SafeAreaEnabled = true
        };

        // Act & Assert
        Assert.Throws<InvalidIdError>(() => command.ApplyToState(state));
    }

    [Test]
    public void ApplyToState_WithEmptyMultiViewerState_ThrowsInvalidIdError()
    {
        // Arrange
        var state = new AtemState(); // Empty state
        var command = new MultiViewerWindowSafeAreaUpdateCommand
        {
            MultiViewerId = 0,
            WindowIndex = 0,
            SafeAreaEnabled = true
        };

        // Act & Assert
        Assert.Throws<InvalidIdError>(() => command.ApplyToState(state));
    }

    protected override void CompareCommandProperties(MultiViewerWindowSafeAreaUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.MultiViewerId, Is.EqualTo(expectedData.MultiviewIndex));
        Assert.That(actualCommand.WindowIndex, Is.EqualTo(expectedData.WindowIndex));
        Assert.That(actualCommand.SafeAreaEnabled, Is.EqualTo(expectedData.SafeAreaEnabled));
    }
}
