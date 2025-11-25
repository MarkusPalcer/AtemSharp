using AtemSharp.Commands.MixEffects;
using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Tests.Commands.MixEffects;

[TestFixture]
public class ProgramInputCommandTests : SerializedCommandTestBase<ProgramInputCommand,
    ProgramInputCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public ushort Source { get; set; }
    }

    protected override ProgramInputCommand CreateSut(TestCaseData testCase)
    {
        return new ProgramInputCommand(new MixEffect
        {
            Id = testCase.Command.Index,
            ProgramInput = testCase.Command.Source
        });
    }

    [Test]
    public void Constructor_WithValidMixEffect_InitializesFromState()
    {
        // Arrange
        const int mixEffectId = 1;
        const int expectedSource = 1234;
        var state = new MixEffect
        {
            Id = mixEffectId,
            ProgramInput = expectedSource
        };


        // Act
        var command = new ProgramInputCommand(state);

        // Assert
        Assert.That(command.MixEffectId, Is.EqualTo(mixEffectId));
        Assert.That(command.Source, Is.EqualTo(expectedSource));
        Assert.That(command.Flag, Is.EqualTo(0), "Flag should not be set when initializing from state");
    }
}
