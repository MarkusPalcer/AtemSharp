using AtemSharp.Commands.DownstreamKey;
using AtemSharp.State.Video.DownstreamKeyer;

namespace AtemSharp.Tests.Commands.DownstreamKey;

[TestFixture]
public class DownstreamKeyGeneralCommandTests : SerializedCommandTestBase<DownstreamKeyGeneralCommand,
    DownstreamKeyGeneralCommandTests.CommandData>
{
    protected override Range[] GetFloatingPointByteRanges() => [
        (4..6), // Clip
        (6..8) // Gain
    ];

    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public bool PreMultipliedKey { get; set; }
        public double Clip { get; set; }
        public double Gain { get; set; }
        public bool Invert { get; set; }
    }

    protected override DownstreamKeyGeneralCommand CreateSut(TestCaseData testCase)
    {
        return new DownstreamKeyGeneralCommand(new DownstreamKeyer
        {
            Id = testCase.Command.Index,
            Properties =
            {
                Gain =  testCase.Command.Gain,
                Clip = testCase.Command.Clip,
                Invert =  testCase.Command.Invert,
                PreMultiply = testCase.Command.PreMultipliedKey
            }
        });
    }

    [Test]
    public void PreMultiply_WhenSet_ShouldUpdateFlag()
    {
        // Arrange
        var command = new DownstreamKeyGeneralCommand(new DownstreamKeyer());

        // Act
        command.PreMultiply = true;

        // Assert
        Assert.That(command.Flag & 1, Is.EqualTo(1)); // Flag bit 0 should be set
    }

    [Test]
    public void Clip_WhenSet_ShouldUpdateFlag()
    {
        // Arrange
        var command = new DownstreamKeyGeneralCommand(new DownstreamKeyer());

        // Act
        command.Clip = 50.0;

        // Assert
        Assert.That(command.Flag & 2, Is.EqualTo(2)); // Flag bit 1 should be set
    }

    [Test]
    public void Gain_WhenSet_ShouldUpdateFlagAndValue()
    {
        // Arrange
        var command = new DownstreamKeyGeneralCommand(new DownstreamKeyer());

        // Act
        command.Gain = 75.0;

        // Assert
        Assert.That(command.Flag & 4, Is.EqualTo(4)); // Flag bit 2 should be set
    }

    [Test]
    public void Properties_WhenSetMultipleTimes_ShouldMaintainFlags()
    {
        // Arrange
        var command = new DownstreamKeyGeneralCommand(new DownstreamKeyer());

        // Act
        command.PreMultiply = true;
        command.Clip = 25.0;
        command.PreMultiply = false; // Change again
        command.Gain = 50.0;

        // Assert
        Assert.That(command.Flag, Is.EqualTo(7)); // Bits 0, 1, 2 should be set (1 + 2 + 4 = 7)
    }
}
