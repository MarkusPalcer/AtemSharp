using AtemSharp.Commands.Settings;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Settings;

[TestFixture]
public class MultiViewerVuOpacityCommandTests : SerializedCommandTestBase<MultiViewerVuOpacityCommand,
    MultiViewerVuOpacityCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int MultiviewIndex { get; set; } // Match TypeScript property name
        public int Opacity { get; set; }
    }

    protected override MultiViewerVuOpacityCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required MultiViewer
        var state = CreateStateWithMultiViewer(testCase.Command.MultiviewIndex);

        // Create command with the MultiViewer ID
        var command = new MultiViewerVuOpacityCommand(testCase.Command.MultiviewIndex, state);

        // Set the actual values that should be written
        command.Opacity = testCase.Command.Opacity;

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
        Assert.Throws<InvalidIdError>(() => _ = new MultiViewerVuOpacityCommand(multiViewerId, state));
    }

    [Test]
    public void Constructor_WithValidState_InitializesWithDefaults()
    {
        // Arrange
        const int multiViewerId = 1;
        var state = CreateStateWithMultiViewer(multiViewerId);

        // Act
        var command = new MultiViewerVuOpacityCommand(multiViewerId, state);

        // Assert
        Assert.That(command.MultiViewerId, Is.EqualTo(multiViewerId));
        Assert.That(command.Opacity, Is.EqualTo(0)); // Default from state
    }

    [Test]
    public void ConvenienceConstructor_WithValues_SetsProperties()
    {
        // Arrange
        const int multiViewerId = 2;
        const int opacity = 75;
        var state = CreateStateWithMultiViewer(multiViewerId);

        // Act
        var command = new MultiViewerVuOpacityCommand(multiViewerId, opacity, state);

        // Assert
        Assert.That(command.MultiViewerId, Is.EqualTo(multiViewerId));
        Assert.That(command.Opacity, Is.EqualTo(opacity));
    }

    [Test]
    [TestCase(-1)]
    [TestCase(101)]
    public void Opacity_WithInvalidValue_ThrowsArgumentOutOfRangeException(int invalidOpacity)
    {
        // Arrange
        const int multiViewerId = 1;
        var state = CreateStateWithMultiViewer(multiViewerId);
        var command = new MultiViewerVuOpacityCommand(multiViewerId, state);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => command.Opacity = invalidOpacity);
    }

    [Test]
    [TestCase(0)]
    [TestCase(50)]
    [TestCase(100)]
    public void Opacity_WithValidValue_SetsProperty(int validOpacity)
    {
        // Arrange
        const int multiViewerId = 1;
        var state = CreateStateWithMultiViewer(multiViewerId);
        var command = new MultiViewerVuOpacityCommand(multiViewerId, state);

        // Act
        command.Opacity = validOpacity;

        // Assert
        Assert.That(command.Opacity, Is.EqualTo(validOpacity));
    }

    #region Deserialization Tests

    [Test]
    public void Deserialize_WithValidData_ReturnsCorrectCommand()
    {
        // Arrange
        var bytes = new byte[] { 1, 75, 0, 0 }; // multiViewerId=1, opacity=75
        using var stream = new MemoryStream(bytes);

        // Act
        var result = MultiViewerVuOpacityCommand.Deserialize(stream, ProtocolVersion.V8_0);

        // Assert
        Assert.That(result.MultiViewerId, Is.EqualTo(1));
        Assert.That(result.Opacity, Is.EqualTo(75));
    }

    [Test]
    public void Deserialize_WithZeroOpacity_ReturnsCorrectCommand()
    {
        // Arrange
        var bytes = new byte[] { 0, 0, 0, 0 }; // multiViewerId=0, opacity=0
        using var stream = new MemoryStream(bytes);

        // Act
        var result = MultiViewerVuOpacityCommand.Deserialize(stream, ProtocolVersion.V8_0);

        // Assert
        Assert.That(result.MultiViewerId, Is.EqualTo(0));
        Assert.That(result.Opacity, Is.EqualTo(0));
    }

    [Test]
    public void Deserialize_WithMaxOpacity_ReturnsCorrectCommand()
    {
        // Arrange
        var bytes = new byte[] { 2, 100, 0, 0 }; // multiViewerId=2, opacity=100
        using var stream = new MemoryStream(bytes);

        // Act
        var result = MultiViewerVuOpacityCommand.Deserialize(stream, ProtocolVersion.V8_0);

        // Assert
        Assert.That(result.MultiViewerId, Is.EqualTo(2));
        Assert.That(result.Opacity, Is.EqualTo(100));
    }

    #endregion

    #region ApplyToState Tests

    [Test]
    public void ApplyToState_WithValidMultiViewer_UpdatesVuOpacity()
    {
        // Arrange
        const int multiViewerId = 1;
        const int opacity = 85;
        
        var state = CreateStateWithMultiViewer(multiViewerId);
        var command = new MultiViewerVuOpacityCommand
        {
            MultiViewerId = multiViewerId,
            Opacity = opacity
        };

        // Act
        var changedPaths = command.ApplyToState(state);

        // Assert
        var multiViewer = AtemStateUtil.GetMultiViewer(state, multiViewerId);
        Assert.That(multiViewer.VuOpacity, Is.EqualTo(opacity), "VuOpacity should be updated");
        
        // Verify the returned path
        Assert.That(changedPaths, Is.Not.Null);
        Assert.That(changedPaths.Length, Is.EqualTo(1));
        Assert.That(changedPaths[0], Is.EqualTo($"settings.multiViewers.{multiViewerId}.vuOpacity"));
    }

    [Test]
    public void ApplyToState_WithExistingOpacity_OverwritesPreviousValue()
    {
        // Arrange
        const int multiViewerId = 1;
        const int oldOpacity = 50;
        const int newOpacity = 90;
        
        var state = CreateStateWithMultiViewer(multiViewerId);
        var multiViewer = AtemStateUtil.GetMultiViewer(state, multiViewerId);
        
        // Pre-populate with existing opacity
        multiViewer.VuOpacity = oldOpacity;

        var command = new MultiViewerVuOpacityCommand
        {
            MultiViewerId = multiViewerId,
            Opacity = newOpacity
        };

        // Act
        _ = command.ApplyToState(state);

        // Assert
        Assert.That(multiViewer.VuOpacity, Is.EqualTo(newOpacity), "VuOpacity should be updated to new value");
    }

    [Test]
    public void ApplyToState_WithInvalidMultiViewer_ThrowsInvalidIdError()
    {
        // Arrange
        const int multiViewerId = 5; // Not in state
        var state = CreateStateWithMultiViewer(1); // Only has MultiViewer 1
        var command = new MultiViewerVuOpacityCommand
        {
            MultiViewerId = multiViewerId,
            Opacity = 50
        };

        // Act & Assert
        Assert.Throws<InvalidIdError>(() => command.ApplyToState(state));
    }

    [Test]
    public void ApplyToState_WithEmptyMultiViewerState_ThrowsInvalidIdError()
    {
        // Arrange
        var state = new AtemState(); // Empty state
        var command = new MultiViewerVuOpacityCommand
        {
            MultiViewerId = 0,
            Opacity = 50
        };

        // Act & Assert
        Assert.Throws<InvalidIdError>(() => command.ApplyToState(state));
    }

    #endregion
}