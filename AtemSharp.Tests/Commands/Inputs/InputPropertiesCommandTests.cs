using AtemSharp.Commands.Inputs;
using AtemSharp.State.Ports;
using AtemSharp.State.Video.InputChannel;

namespace AtemSharp.Tests.Commands.Inputs;

[TestFixture]
public class InputPropertiesCommandTests : SerializedCommandTestBase<InputPropertiesCommand,
    InputPropertiesCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Id { get; set; }
        public string LongName { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public ExternalPortType ExternalPortType { get; set; }
    }

    protected override InputPropertiesCommand CreateSut(TestCaseData testCase)
    {
        return new InputPropertiesCommand(new InputChannel
        {
            InputId = testCase.Command.Id,
            LongName = testCase.Command.LongName,
            ShortName = testCase.Command.ShortName,
            ExternalPortType = testCase.Command.ExternalPortType
        });
    }

    [Test]
    public void LongName_WhenSet_UpdatesFlagAutomatically()
    {
        // Arrange
        var command = new InputPropertiesCommand(new InputChannel());

        // Act
        command.LongName = "New Long Name";

        // Assert
        Assert.That(command.Flag, Is.EqualTo(1), "Flag bit 0 should be set for LongName");
    }

    [Test]
    public void ShortName_WhenSet_UpdatesFlagAutomatically()
    {
        // Arrange
        var command = new InputPropertiesCommand(new InputChannel());

        // Act
        command.ShortName = "NEW";

        // Assert
        Assert.That(command.Flag, Is.EqualTo(2), "Flag bit 1 should be set for ShortName");
    }

    [Test]
    public void ExternalPortType_WhenSet_UpdatesFlagAutomatically()
    {
        // Arrange
        var command = new InputPropertiesCommand(new InputChannel());

        // Act
        command.ExternalPortType = ExternalPortType.HDMI;

        // Assert
        Assert.That(command.Flag, Is.EqualTo(4), "Flag bit 2 should be set for ExternalPortType");
    }

    [Test]
    public void Properties_WhenMultipleSet_CombineFlags()
    {
        // Arrange
        var command = new InputPropertiesCommand(new InputChannel());

        // Act
        command.LongName = "Test Long";
        command.ExternalPortType = ExternalPortType.HDMI;

        // Assert
        Assert.That(command.Flag, Is.EqualTo(5), "Flag should combine bits 0 and 2 (1 + 4 = 5)");
    }
}
