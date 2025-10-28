using AtemSharp.Commands.DownstreamKey;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DownstreamKey;

[TestFixture]
public class DownstreamKeyOnAirCommandTests : SerializedCommandTestBase<DownstreamKeyOnAirCommand,
    DownstreamKeyOnAirCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public bool OnAir { get; set; }
    }

    protected override DownstreamKeyOnAirCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required downstream keyer
        var state = CreateDownstreamKeyer(testCase.Command.Index);

        // Create command with the keyer ID
        var command = new DownstreamKeyOnAirCommand(state);

        // Set the actual values that should be written
        command.OnAir = testCase.Command.OnAir;

        return command;
    }

    /// <summary>
    /// Creates an AtemState with a valid downstream keyer at the specified index
    /// </summary>
    private static DownstreamKeyer CreateDownstreamKeyer(byte keyerId)
    {
        return new DownstreamKeyer
        {
            Id = keyerId,
            InTransition = false,
            RemainingFrames = 0,
            IsAuto = false,
            OnAir = false,
            IsTowardsOnAir = false
        };
    }

    [Test]
    public void Constructor_WithValidKeyerId_ShouldInitializeCorrectly()
    {
        // Arrange
        const int keyerId = 1;
        var state = CreateDownstreamKeyer(keyerId);

        // Act
        var command = new DownstreamKeyOnAirCommand(state);

        // Assert
        Assert.That(command.DownstreamKeyerId, Is.EqualTo(keyerId));
        Assert.That(command.OnAir, Is.False); // Default from state
        Assert.That(command.Flag, Is.EqualTo(0)); // No flags set initially
    }
}
