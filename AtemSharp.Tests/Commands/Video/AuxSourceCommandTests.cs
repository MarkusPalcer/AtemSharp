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
        return new AuxSourceCommand(new AuxiliaryOutput {Id = testCase.Command.Id, Source = testCase.Command.Source});
    }

    [Test]
    public void Constructor_DoesNotSetFlag()
    {
        // Act
        var command = new AuxSourceCommand(new AuxiliaryOutput {Id = 0, Source = 100});

        // Assert
        Assert.That(command.Flag, Is.EqualTo(0), "Flag should not be set when constructor sets source property");
    }

    [Test]
    public void Source_WhenSet_UpdatesFlagAutomatically()
    {
        // Arrange
        var command = new AuxSourceCommand(new AuxiliaryOutput {Id = 0, Source = 100});

        // Act
        command.Source = 2000;

        // Assert
        Assert.That(command.Source, Is.EqualTo(2000));
        Assert.That(command.Flag, Is.EqualTo(1), "Flag should be set when Source property is changed");
    }
}
