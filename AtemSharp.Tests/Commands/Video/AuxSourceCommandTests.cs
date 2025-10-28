using AtemSharp.Commands.Video;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Video;

[TestFixture]
public class AuxSourceCommandTests : SerializedCommandTestBase<AuxSourceCommand,
    AuxSourceCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Id { get; set; }
        public ushort Source { get; set; }
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
    private static AtemState CreateStateWithAuxiliary(byte auxId, ushort source = 0)
    {
        return new AtemState
        {
            Video = new VideoState
            {
                Auxiliaries =
                {
                    { auxId, source }
                }
            },
            Info = new DeviceInfo
            {
                Capabilities = new AtemCapabilities
                {
                    Auxiliaries = auxId+1
                }
            }
        };
    }

    [Test]
    public void Constructor_WithEmptyState_Throws()
    {
        // Arrange
        const int auxId = 1;
        var state = new AtemState(); // Empty state

        // Act
        Assert.Throws<IndexOutOfRangeException>(() => _ = new AuxSourceCommand(auxId, state),
                      $"Constructor should throw when auxiliary {auxId} does not exist in state");
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
