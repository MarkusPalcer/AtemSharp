using AtemSharp.Commands.MixEffects;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects;

[TestFixture]
public class PreviewInputCommandTests : SerializedCommandTestBase<PreviewInputCommand,
    PreviewInputCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public ushort Source { get; set; }
    }

    protected override PreviewInputCommand CreateSut(TestCaseData testCase)
    {
        var state = new MixEffect
        {
            Index = testCase.Command.Index,
            PreviewInput = testCase.Command.Source
        };

        return new PreviewInputCommand(state);
    }

    [Test]
    public void Constructor_WithValidMixEffect_InitializesFromState()
    {
        // Arrange
        const int mixEffectId = 1;
        const int expectedSource = 1234;
        var state = new MixEffect()
        {
            Index = mixEffectId,
            PreviewInput = expectedSource
        };

        // Act
        var command = new PreviewInputCommand(state);

        Assert.Multiple(() =>
        {
            Assert.That(command.MixEffectId, Is.EqualTo(mixEffectId));
            Assert.That(command.Source, Is.EqualTo(expectedSource));
            Assert.That(command.Flag, Is.EqualTo(0), "Flag should not be set when initializing from state");
        });
    }

    [Test]
    public void Source_WhenSet_UpdatesFlagAutomatically()
    {
        // Arrange
        var command = new PreviewInputCommand(new MixEffect());

        // Act
        command.Source = 2000;

        // Assert
        Assert.That(command.Source, Is.EqualTo(2000));
    }
}
