using AtemSharp.Commands.Settings.MultiViewers;
using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Tests.Commands.Settings.MultiViewers;

[TestFixture]
public class MultiViewerSourceUpdateCommandTests : DeserializedCommandTestBase<MultiViewerSourceUpdateCommand,
    MultiViewerSourceUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int MultiviewIndex { get; set; }  // Match TypeScript property name
        public int WindowIndex { get; set; }
        public int Source { get; set; }
        public bool SupportVuMeter { get; set; }  // Match TypeScript property name (no 's')
        public bool SupportsSafeArea { get; set; }
    }

    protected override void CompareCommandProperties(MultiViewerSourceUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.MultiViewerId, Is.EqualTo(expectedData.MultiviewIndex),
                   $"MultiViewerId should match expected value for test case {testCase.Name}");
        Assert.That(actualCommand.WindowIndex, Is.EqualTo(expectedData.WindowIndex),
                   $"WindowIndex should match expected value for test case {testCase.Name}");
        Assert.That(actualCommand.Source, Is.EqualTo(expectedData.Source),
                   $"Source should match expected value for test case {testCase.Name}");
        Assert.That(actualCommand.SupportsVuMeter, Is.EqualTo(expectedData.SupportVuMeter),
                   $"SupportsVuMeter should match expected value for test case {testCase.Name}");
        Assert.That(actualCommand.SupportsSafeArea, Is.EqualTo(expectedData.SupportsSafeArea),
                   $"SupportsSafeArea should match expected value for test case {testCase.Name}");
    }

    [Test]
    public void ApplyToState_WithValidMultiViewer_UpdatesWindow()
    {
        // Arrange
        const int multiViewerId = 1;
        const int windowIndex = 5;
        const int newSource = 2000;

        var state = CreateStateWithMultiViewer(multiViewerId);
        var command = new MultiViewerSourceUpdateCommand
        {
            MultiViewerId = multiViewerId,
            WindowIndex = windowIndex,
            Source = newSource,
            SupportsVuMeter = true,
            SupportsSafeArea = false
        };

        // Act
        command.ApplyToState(state);

        // Assert
        var multiViewer = AtemStateUtil.GetMultiViewer(state, multiViewerId);
        Assert.That(multiViewer.Windows.ContainsKey(windowIndex), Is.True, "Window should exist");

        var window = multiViewer.Windows[windowIndex];
        Assert.That(window.Source, Is.EqualTo(newSource));
        Assert.That(window.WindowIndex, Is.EqualTo(windowIndex));
        Assert.That(window.SupportsVuMeter, Is.True);
        Assert.That(window.SupportsSafeArea, Is.False);
    }

    [Test]
    public void ApplyToState_WithoutMultiViewer_ThrowsInvalidIdError()
    {
        // Arrange
        const int multiViewerId = 3;

        var state = new AtemState(); // Empty state
        var command = new MultiViewerSourceUpdateCommand
        {
            MultiViewerId = multiViewerId,
            WindowIndex = 0,
            Source = 1000,
            SupportsVuMeter = false,
            SupportsSafeArea = true
        };

        // Act & Assert
        Assert.Throws<InvalidIdError>(() => command.ApplyToState(state));
    }

    [Test]
    public void ApplyToState_WithExistingWindow_PreservesOptionalProperties()
    {
        // Arrange
        const int multiViewerId = 0;
        const int windowIndex = 3;
        const int newSource = 1500;

        var state = CreateStateWithMultiViewer(multiViewerId);

        // Add existing window with optional properties
        var existingWindow = new MultiViewerWindowState
        {
            WindowIndex = windowIndex,
            Source = 1000,
            SupportsVuMeter = false,
            SupportsSafeArea = false,
            SafeTitle = true,
            AudioMeter = false
        };
        var multiViewer = AtemStateUtil.GetMultiViewer(state, multiViewerId);
        multiViewer.Windows[windowIndex] = existingWindow;

        var command = new MultiViewerSourceUpdateCommand
        {
            MultiViewerId = multiViewerId,
            WindowIndex = windowIndex,
            Source = newSource,
            SupportsVuMeter = true,
            SupportsSafeArea = true
        };

        // Act
        command.ApplyToState(state);

        // Assert
        var window = multiViewer.Windows[windowIndex];
        Assert.That(window.Source, Is.EqualTo(newSource), "Source should be updated");
        Assert.That(window.SupportsVuMeter, Is.True, "SupportsVuMeter should be updated");
        Assert.That(window.SupportsSafeArea, Is.True, "SupportsSafeArea should be updated");
        Assert.That(window.SafeTitle, Is.True, "SafeTitle should be preserved");
        Assert.That(window.AudioMeter, Is.False, "AudioMeter should be preserved");
    }

    [Test]
    public void ApplyToState_WithInvalidMultiViewerId_ThrowsInvalidIdError()
    {
        // Arrange
        const int validMultiViewerId = 0;
        const int invalidMultiViewerId = 5;

        var state = CreateStateWithMultiViewer(validMultiViewerId);
        var command = new MultiViewerSourceUpdateCommand
        {
            MultiViewerId = invalidMultiViewerId,
            WindowIndex = 0,
            Source = 1000,
            SupportsVuMeter = false,
            SupportsSafeArea = true
        };

        // Act & Assert
        Assert.Throws<InvalidIdError>(() => command.ApplyToState(state));
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
}
