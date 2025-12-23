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

    protected override DownstreamKeyCutSourceCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new DownstreamKeyCutSourceCommand(new DownstreamKeyer
        {
            Id = testCase.Command.Index,
            Sources =
            {
                CutSource = testCase.Command.CutSource
            }
        });
    }

    [Test]
    public void Constructor_ShouldInitializeCorrectly()
    {
        // Act
        var command = new DownstreamKeyCutSourceCommand(new DownstreamKeyer());

        // Assert
        Assert.That(command.Flag, Is.EqualTo(0)); // No flags set initially
    }

    [Test]
    public void Input_WhenSet_ShouldUpdateFlag()
    {
        // Arrange
        var command = new DownstreamKeyCutSourceCommand(new DownstreamKeyer());

        // Act
        command.Input = 1234;

        // Assert
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
        Assert.That(command.Flag, Is.EqualTo(1)); // Flag should remain set
        Assert.That(command.Flag, Is.EqualTo(1)); // Flag should remain set
    }
}
