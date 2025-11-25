using AtemSharp.Commands.DownstreamKey;
using AtemSharp.State.Video.DownstreamKeyer;

namespace AtemSharp.Tests.Commands.DownstreamKey;

[TestFixture]
public class DownstreamKeyCutSourceCommandTests : SerializedCommandTestBase<DownstreamKeyCutSourceCommand,
    DownstreamKeyCutSourceCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public ushort CutSource { get; set; }
    }

    protected override DownstreamKeyCutSourceCommand CreateSut(TestCaseData testCase)
    {
        var command = new DownstreamKeyCutSourceCommand(new DownstreamKeyer
        {
            Id = testCase.Command.Index,
            Sources =
            {
                CutSource = testCase.Command.CutSource
            }
        });

        return command;
    }

    [Test]
    public void Constructor_WithValidKeyerId_ShouldInitializeCorrectly()
    {
        // Arrange
        const int keyerId = 1;
        const int expectedCutSource = 42;
        // Act
        var command = new DownstreamKeyCutSourceCommand(new DownstreamKeyer()
        {
            Id = keyerId,
            Sources =
            {
                CutSource = expectedCutSource
            }
        });

        // Assert
        Assert.That(command.DownstreamKeyerId, Is.EqualTo(keyerId));
        Assert.That(command.Input, Is.EqualTo(expectedCutSource)); // Should get value from state
        Assert.That(command.Flag, Is.EqualTo(0)); // No flags set initially
    }

    [Test]
    public void Input_WhenSet_ShouldUpdateFlagAndValue()
    {
        // Arrange
        var command = new DownstreamKeyCutSourceCommand(new DownstreamKeyer());

        // Act
        command.Input = 1234;

        // Assert
        Assert.That(command.Input, Is.EqualTo(1234));
        Assert.That(command.Flag, Is.EqualTo(1)); // Flag bit 0 should be set
    }

    [Test]
    public void Input_WhenSetMultipleTimes_ShouldMaintainFlag()
    {
        // Arrange
        var command = new DownstreamKeyCutSourceCommand(new DownstreamKeyer());

        // Act
        command.Input = 100;
        command.Input = 200;

        // Assert
        Assert.That(command.Input, Is.EqualTo(200));
        Assert.That(command.Flag, Is.EqualTo(1)); // Flag should remain set
    }
}
