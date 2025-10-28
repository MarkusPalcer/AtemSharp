using AtemSharp.Commands.Inputs;
using AtemSharp.Enums.Ports;
using AtemSharp.State;

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
        // Create state with the required input channel
        var state = CreateInput(testCase.Command.Id,
            testCase.Command.LongName,
            testCase.Command.ShortName,
            testCase.Command.ExternalPortType);

        // Create command with the input ID
        var command = new InputPropertiesCommand(state);

        // Set the properties from test data
        command.LongName = testCase.Command.LongName;
        command.ShortName = testCase.Command.ShortName;
        command.ExternalPortType = testCase.Command.ExternalPortType;

        return command;
    }

    /// <summary>
    /// Creates an AtemState with a valid input channel at the specified index
    /// </summary>
    private static InputChannel CreateInput(ushort inputId, string longName = "", string shortName = "", ExternalPortType externalPortType = ExternalPortType.Unknown)
    {
        return new InputChannel
        {
            InputId = inputId,
            LongName = longName,
            ShortName = shortName,
            ExternalPortType = externalPortType
        };
    }

    [Test]
    public void Constructor_WithEmptyState_InitializesWithDefaults()
    {
        // Arrange
        const int inputId = 1;

        // Act
        var command = new InputPropertiesCommand(CreateInput(inputId));

        // Assert
        Assert.That(command.InputId, Is.EqualTo(inputId));
        Assert.That(command.LongName, Is.EqualTo(string.Empty));
        Assert.That(command.ShortName, Is.EqualTo(string.Empty));
        Assert.That(command.ExternalPortType, Is.EqualTo(ExternalPortType.Unknown));
        Assert.That(command.Flag, Is.EqualTo(0), "No");
    }

    [Test]
    public void Constructor_WithExistingState_InitializesFromState()
    {
        // Arrange
        const int inputId = 2;
        const string longName = "Camera 1";
        const string shortName = "CAM1";
        const ExternalPortType externalPortType = ExternalPortType.HDMI;
        var state = CreateInput(inputId, longName, shortName, externalPortType);

        // Act
        var command = new InputPropertiesCommand(state);

        // Assert
        Assert.That(command.InputId, Is.EqualTo(inputId));
        Assert.That(command.LongName, Is.EqualTo(longName));
        Assert.That(command.ShortName, Is.EqualTo(shortName));
        Assert.That(command.ExternalPortType, Is.EqualTo(externalPortType));
        Assert.That(command.Flag, Is.EqualTo(0), "Flag should not be set when initializing from existing state");
    }

    [Test]
    public void LongName_WhenSet_UpdatesFlagAutomatically()
    {
        // Arrange
        const int inputId = 0;
        var state = CreateInput(inputId, "Original", "ORIG", ExternalPortType.SDI);
        var command = new InputPropertiesCommand(state);

        // Reset flag after constructor
        command.Flag = 0;

        // Act
        command.LongName = "New Long Name";

        // Assert
        Assert.That(command.LongName, Is.EqualTo("New Long Name"));
        Assert.That(command.Flag, Is.EqualTo(1), "Flag bit 0 should be set for LongName");
    }

    [Test]
    public void ShortName_WhenSet_UpdatesFlagAutomatically()
    {
        // Arrange
        const int inputId = 0;
        var state = CreateInput(inputId, "Original", "ORIG", ExternalPortType.SDI);
        var command = new InputPropertiesCommand(state);

        // Reset flag after constructor
        command.Flag = 0;

        // Act
        command.ShortName = "NEW";

        // Assert
        Assert.That(command.ShortName, Is.EqualTo("NEW"));
        Assert.That(command.Flag, Is.EqualTo(2), "Flag bit 1 should be set for ShortName");
    }

    [Test]
    public void ExternalPortType_WhenSet_UpdatesFlagAutomatically()
    {
        // Arrange
        const int inputId = 0;
        var state = CreateInput(inputId, "Original", "ORIG", ExternalPortType.SDI);
        var command = new InputPropertiesCommand(state);

        // Reset flag after constructor
        command.Flag = 0;

        // Act
        command.ExternalPortType = ExternalPortType.HDMI;

        // Assert
        Assert.That(command.ExternalPortType, Is.EqualTo(ExternalPortType.HDMI));
        Assert.That(command.Flag, Is.EqualTo(4), "Flag bit 2 should be set for ExternalPortType");
    }

    [Test]
    public void Properties_WhenMultipleSet_CombineFlags()
    {
        // Arrange
        const int inputId = 0;
        var state = CreateInput(inputId);
        var command = new InputPropertiesCommand(state);

        // Reset flag after constructor
        command.Flag = 0;

        // Act
        command.LongName = "Test Long";
        command.ExternalPortType = ExternalPortType.HDMI;

        // Assert
        Assert.That(command.Flag, Is.EqualTo(5), "Flag should combine bits 0 and 2 (1 + 4 = 5)");
    }
}
