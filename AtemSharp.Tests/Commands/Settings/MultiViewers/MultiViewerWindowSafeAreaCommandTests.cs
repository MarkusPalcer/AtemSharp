using AtemSharp.Commands.Settings.MultiViewers;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Settings.MultiViewers;

[TestFixture]
public class MultiViewerWindowSafeAreaCommandTests : SerializedCommandTestBase<MultiViewerWindowSafeAreaCommand,
    MultiViewerWindowSafeAreaCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MultiviewIndex { get; set; } // Match TypeScript property name
        public byte WindowIndex { get; set; }
        public bool SafeAreaEnabled { get; set; }
    }

    protected override MultiViewerWindowSafeAreaCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required MultiViewer
        var state = new MultiViewerWindowState
        {
            MultiViewerId = testCase.Command.MultiviewIndex,
            WindowIndex = testCase.Command.WindowIndex,
            SafeTitle = testCase.Command.SafeAreaEnabled
        };

        // Create command with the MultiViewer ID
        var command = new MultiViewerWindowSafeAreaCommand(state);

        // Set the actual values that should be written
        command.WindowIndex = testCase.Command.WindowIndex;
        command.SafeAreaEnabled = testCase.Command.SafeAreaEnabled;

        return command;
    }

    [Test]
    public void Constructor_InitializesFromState()
    {
        // Arrange
        const int multiViewerId = 1;
        var state = new MultiViewerWindowState
        {
            MultiViewerId = 1,
            WindowIndex = 2,
            SafeTitle = true
        };

        // Act
        var command = new MultiViewerWindowSafeAreaCommand(state);

        // Assert
        Assert.That(command.MultiViewerId, Is.EqualTo(multiViewerId));
        Assert.That(command.WindowIndex, Is.EqualTo(2));
        Assert.That(command.SafeAreaEnabled, Is.True);
    }
}
