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
        public int WindowIndex { get; set; }
        public bool SafeAreaEnabled { get; set; }
    }


    private static AtemState CreateStateWithMultiViewer(byte multiViewerId)
    {
        return new AtemState
        {
            Settings =
            {
                MultiViewers = AtemStateUtil.CreateArray<MultiViewer>(multiViewerId + 1)
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
        var multiViewer = state.Settings.MultiViewers[multiViewerId];
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
        var multiViewer = state.Settings.MultiViewers[multiViewerId];

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

    protected override void CompareCommandProperties(MultiViewerWindowSafeAreaUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.MultiViewerId, Is.EqualTo(expectedData.MultiviewIndex));
        Assert.That(actualCommand.WindowIndex, Is.EqualTo(expectedData.WindowIndex));
        Assert.That(actualCommand.SafeAreaEnabled, Is.EqualTo(expectedData.SafeAreaEnabled));
    }
}
