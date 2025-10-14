using AtemSharp.Commands.Settings;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Settings;

[TestFixture]
public class MultiViewerWindowSafeAreaCommandTests : SerializedCommandTestBase<MultiViewerWindowSafeAreaCommand,
    MultiViewerWindowSafeAreaCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int MultiviewIndex { get; set; } // Match TypeScript property name
        public int WindowIndex { get; set; }
        public bool SafeAreaEnabled { get; set; }
    }

    protected override MultiViewerWindowSafeAreaCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required MultiViewer
        var state = CreateStateWithMultiViewer(testCase.Command.MultiviewIndex);

        // Create command with the MultiViewer ID
        var command = new MultiViewerWindowSafeAreaCommand(testCase.Command.MultiviewIndex, state);

        // Set the actual values that should be written
        command.WindowIndex = testCase.Command.WindowIndex;
        command.SafeAreaEnabled = testCase.Command.SafeAreaEnabled;

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
        Assert.Throws<InvalidIdError>(() => _ = new MultiViewerWindowSafeAreaCommand(multiViewerId, state));
    }

    [Test]
    public void Constructor_WithValidState_InitializesWithDefaults()
    {
        // Arrange
        const int multiViewerId = 1;
        var state = CreateStateWithMultiViewer(multiViewerId);

        // Act
        var command = new MultiViewerWindowSafeAreaCommand(multiViewerId, state);

        // Assert
        Assert.That(command.MultiViewerId, Is.EqualTo(multiViewerId));
        Assert.That(command.WindowIndex, Is.EqualTo(0));
        Assert.That(command.SafeAreaEnabled, Is.False);
    }

    [Test]
    public void ConvenienceConstructor_WithValues_SetsProperties()
    {
        // Arrange
        const int multiViewerId = 2;
        const int windowIndex = 5;
        const bool safeAreaEnabled = true;
        var state = CreateStateWithMultiViewer(multiViewerId);

        // Act
        var command = new MultiViewerWindowSafeAreaCommand(multiViewerId, windowIndex, safeAreaEnabled, state);

        // Assert
        Assert.That(command.MultiViewerId, Is.EqualTo(multiViewerId));
        Assert.That(command.WindowIndex, Is.EqualTo(windowIndex));
        Assert.That(command.SafeAreaEnabled, Is.EqualTo(safeAreaEnabled));
    }

    #region Deserialization Tests

    [Test]
    public void Deserialize_WithValidData_ReturnsCorrectCommand()
    {
        // Arrange
        var bytes = new byte[] { 235, 8, 1, 0 }; // From test data: multiViewerId=235, windowIndex=8, safeAreaEnabled=true
        using var stream = new MemoryStream(bytes);

        // Act
        var result = MultiViewerWindowSafeAreaCommand.Deserialize(stream, ProtocolVersion.V8_0);

        // Assert
        Assert.That(result.MultiViewerId, Is.EqualTo(235));
        Assert.That(result.WindowIndex, Is.EqualTo(8));
        Assert.That(result.SafeAreaEnabled, Is.True);
    }

    [Test]
    public void Deserialize_WithFalseValue_ReturnsCorrectCommand()
    {
        // Arrange
        var bytes = new byte[] { 196, 13, 0, 0 }; // From test data: multiViewerId=196, windowIndex=13, safeAreaEnabled=false
        using var stream = new MemoryStream(bytes);

        // Act
        var result = MultiViewerWindowSafeAreaCommand.Deserialize(stream, ProtocolVersion.V8_0);

        // Assert
        Assert.That(result.MultiViewerId, Is.EqualTo(196));
        Assert.That(result.WindowIndex, Is.EqualTo(13));
        Assert.That(result.SafeAreaEnabled, Is.False);
    }

    #endregion

    #region ApplyToState Tests

    [Test]
    public void ApplyToState_WithValidMultiViewer_UpdatesWindow()
    {
        // Arrange
        const int multiViewerId = 1;
        const int windowIndex = 5;
        const bool safeAreaEnabled = true;

        var state = CreateStateWithMultiViewer(multiViewerId);
        var command = new MultiViewerWindowSafeAreaCommand
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

        var command = new MultiViewerWindowSafeAreaCommand
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
        var command = new MultiViewerWindowSafeAreaCommand
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
        var command = new MultiViewerWindowSafeAreaCommand
        {
            MultiViewerId = 0,
            WindowIndex = 0,
            SafeAreaEnabled = true
        };

        // Act & Assert
        Assert.Throws<InvalidIdError>(() => command.ApplyToState(state));
    }

    #endregion
}
