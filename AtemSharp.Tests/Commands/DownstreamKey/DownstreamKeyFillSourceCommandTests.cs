using AtemSharp.Commands;
using AtemSharp.Commands.DownstreamKey;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DownstreamKey;

[TestFixture]
public class DownstreamKeyFillSourceCommandTests : SerializedCommandTestBase<DownstreamKeyFillSourceCommand,
    DownstreamKeyFillSourceCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public ushort FillSource { get; set; }
    }

    protected override DownstreamKeyFillSourceCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required downstream keyer
        var state = CreateDownstreamKeyer(testCase.Command.Index, testCase.Command.FillSource);

        // Create command with the keyer ID
        var command = new DownstreamKeyFillSourceCommand(state)
        {
	        // Set the actual input value that should be written
	        Input = testCase.Command.FillSource
        };

        return command;
    }

    /// <summary>
    /// Creates an AtemState with a valid downstream keyer at the specified index
    /// </summary>
    private static DownstreamKeyer CreateDownstreamKeyer(byte keyerId, ushort fillSource = 0)
    {
        return new()
	        {
                Id = keyerId,
		        InTransition = false,
		        RemainingFrames = 0,
		        IsAuto = false,
		        OnAir = false,
		        IsTowardsOnAir = false,
		        Sources = new DownstreamKeyerSources
		        {
			        FillSource = fillSource,
			        CutSource = 1000
		        }
        };
    }

    [Test]
    public void Constructor_WithValidKeyerId_ShouldInitializeCorrectly()
    {
        // Arrange
        const byte keyerId = 1;
        const int expectedFillSource = 42;
        var state = CreateDownstreamKeyer(keyerId, expectedFillSource);

        // Act
        var command = new DownstreamKeyFillSourceCommand(state);

        // Assert
        Assert.That(command.DownstreamKeyerId, Is.EqualTo(keyerId));
        Assert.That(command.Input, Is.EqualTo(expectedFillSource)); // Should get value from state
        Assert.That(command.Flag, Is.EqualTo(0)); // No flags set initially
    }
}
