using AtemSharp.Commands.DownstreamKey;
using AtemSharp.State.Video.DownstreamKeyer;

namespace AtemSharp.Tests.Commands.DownstreamKey;

[TestFixture]
public class DownstreamKeyAutoCommandTests : SerializedCommandTestBase<DownstreamKeyAutoCommand,
    DownstreamKeyAutoCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public bool IsTowardsOnAir { get; set; }
    }

    protected override DownstreamKeyAutoCommand CreateSut(TestCaseData testCase)
    {
        return new DownstreamKeyAutoCommand(new DownstreamKeyer
        {
            Id = testCase.Command.Index,
            IsTowardsOnAir = testCase.Command.IsTowardsOnAir
        });
    }

    [Test]
    public void Constructor_WithValidKeyerId_ShouldInitializeCorrectly()
    {
        // Arrange
        // Act
        var command = new DownstreamKeyAutoCommand(new DownstreamKeyer());

        // Assert
        Assert.That(command.Flag, Is.EqualTo(0)); // No flags set initially
    }

    [Test]
    public void IsTowardsOnAir_WhenSet_ShouldUpdateFlag()
    {
        // Arrange
        var command = new DownstreamKeyAutoCommand(new DownstreamKeyer());

        // Act
        command.IsTowardsOnAir = true;

        // Assert
        Assert.That(command.Flag, Is.EqualTo(1)); // Flag bit 0 should be set
    }

    [Test]
    public void IsTowardsOnAir_WhenSetMultipleTimes_ShouldMaintainFlag()
    {
        // Arrange
        var command = new DownstreamKeyAutoCommand(new DownstreamKeyer());

        // Act
        command.IsTowardsOnAir = true;
        command.IsTowardsOnAir = false;

        // Assert
        Assert.That(command.Flag, Is.EqualTo(1)); // Flag should remain set
    }
}
