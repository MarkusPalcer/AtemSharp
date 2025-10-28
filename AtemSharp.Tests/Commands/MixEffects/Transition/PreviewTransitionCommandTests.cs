using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class PreviewTransitionCommandTests : SerializedCommandTestBase<PreviewTransitionCommand,
    PreviewTransitionCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public bool PreviewTransition { get; set; }
    }

    protected override PreviewTransitionCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required mix effect
        var state = new MixEffect
        {
            Index = testCase.Command.Index,
            TransitionPreview = testCase.Command.PreviewTransition
        };

        // Create command with the mix effect ID
        var command = new PreviewTransitionCommand(state);

        return command;
    }

    [Test]
    public void Constructor_WithValidMixEffect_InitializesFromState()
    {
        // Arrange
        var state = new MixEffect
        {
            Index = 1,
            TransitionPreview = true
        };

        // Act
        var command = new PreviewTransitionCommand(state);

        Assert.Multiple(() =>
        {
            Assert.That(command.MixEffectId, Is.EqualTo(1));
            Assert.That(command.Preview, Is.EqualTo(true));
        });
    }
}
