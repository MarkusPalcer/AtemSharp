using AtemSharp.Commands;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands;

[TestFixture]
public class AuxSourceCommandTests : SerializedCommandTestBase<AuxSourceCommand,
    AuxSourceCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Id { get; set; }
        public int Source { get; set; }
    }

    protected override AuxSourceCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required auxiliary output
        var state = CreateStateWithAuxiliary(testCase.Command.Id, testCase.Command.Source);
        
        // Create command with the auxiliary ID
        var command = new AuxSourceCommand(testCase.Command.Id, state);

        // Set the actual source value that should be written
        command.Source = testCase.Command.Source;
        
        return command;
    }

    /// <summary>
    /// Creates an AtemState with a valid auxiliary output at the specified index
    /// </summary>
    private static AtemState CreateStateWithAuxiliary(int auxId, int source = 0)
    {
        var auxiliaries = new int?[Math.Max(auxId + 1, 2)];
        auxiliaries[auxId] = source;

        return new AtemState
        {
            Video = new VideoState
            {
                Auxiliaries = auxiliaries
            },
            Info = new DeviceInfo
            {
                Capabilities = new AtemCapabilities
                {
                    Auxiliaries = auxiliaries.Length
                }
            }
        };
    }

    [Test]
    public void Constructor_WithEmptyState_InitializesWithDefaults()
    {
        // Arrange
        const int auxId = 1;
        var state = new AtemState(); // Empty state

        // Act
        var command = new AuxSourceCommand(auxId, state);

        // Assert
        Assert.That(command.AuxBus, Is.EqualTo(auxId));
        Assert.That(command.Source, Is.EqualTo(0));
        Assert.That(command.Flag, Is.EqualTo(1), "Flag should be set when initializing with defaults");
    }

    [Test]
    public void Constructor_WithExistingState_InitializesFromState()
    {
        // Arrange
        const int auxId = 2;
        const int existingSource = 1500;
        var state = CreateStateWithAuxiliary(auxId, existingSource);

        // Act
        var command = new AuxSourceCommand(auxId, state);

        // Assert
        Assert.That(command.AuxBus, Is.EqualTo(auxId));
        Assert.That(command.Source, Is.EqualTo(existingSource));
        Assert.That(command.Flag, Is.EqualTo(0), "Flag should not be set when initializing from existing state");
    }

    [Test]
    public void Source_WhenSet_UpdatesFlagAutomatically()
    {
        // Arrange
        const int auxId = 0;
        var state = CreateStateWithAuxiliary(auxId, 100);
        var command = new AuxSourceCommand(auxId, state);
        
        // Reset flag after constructor
        command.Flag = 0;

        // Act
        command.Source = 2000;

        // Assert
        Assert.That(command.Source, Is.EqualTo(2000));
        Assert.That(command.Flag, Is.EqualTo(1), "Flag should be set when Source property is changed");
    }
}