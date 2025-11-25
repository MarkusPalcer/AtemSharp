using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State;
using AtemSharp.State.Video;
using AtemSharp.State.Video.MixEffect;
using AtemSharp.State.Video.MixEffect.Transition;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class TransitionPropertiesUpdateCommandTests : DeserializedCommandTestBase<TransitionPropertiesUpdateCommand,
    TransitionPropertiesUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public TransitionStyle Style { get; set; }
        public TransitionSelection Selection { get; set; }
        public TransitionStyle NextStyle { get; set; }
        public TransitionSelection NextSelection { get; set; }
    }

    protected override void CompareCommandProperties(TransitionPropertiesUpdateCommand actualCommand,
                                                     CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.Style, Is.EqualTo(expectedData.Style));
        Assert.That(actualCommand.Selection, Is.EqualTo(expectedData.Selection));
        Assert.That(actualCommand.NextStyle, Is.EqualTo(expectedData.NextStyle));
        Assert.That(actualCommand.NextSelection, Is.EqualTo(expectedData.NextSelection));
    }

    [Test]
    public void ApplyToState_ValidMixEffect_UpdatesTransitionProperties()
    {
        // Arrange
        var command = new TransitionPropertiesUpdateCommand
        {
            MixEffectId = 1,
            Style = TransitionStyle.Wipe,
            Selection = TransitionSelection.Background | TransitionSelection.Key1,
            NextStyle = TransitionStyle.DVE,
            NextSelection = TransitionSelection.Key2 | TransitionSelection.Key3
        };

        var state = CreateMinimalState();

        // Act
        command.ApplyToState(state);

        // Assert
        var mixEffect = state.Video.MixEffects[1];
        Assert.That(mixEffect.TransitionProperties, Is.Not.Null);
        Assert.That(mixEffect.TransitionProperties.Style, Is.EqualTo(TransitionStyle.Wipe));
        Assert.That(mixEffect.TransitionProperties.Selection, Is.EqualTo(TransitionSelection.Background | TransitionSelection.Key1));
        Assert.That(mixEffect.TransitionProperties.NextStyle, Is.EqualTo(TransitionStyle.DVE));
        Assert.That(mixEffect.TransitionProperties.NextSelection, Is.EqualTo(TransitionSelection.Key2 | TransitionSelection.Key3));
    }

    [Test]
    public void ApplyToState_ExistingTransitionProperties_UpdatesInPlace()
    {
        // Arrange
        var command = new TransitionPropertiesUpdateCommand
        {
            MixEffectId = 0,
            Style = TransitionStyle.Sting,
            Selection = TransitionSelection.Key4,
            NextStyle = TransitionStyle.Dip,
            NextSelection = TransitionSelection.Background
        };

        var state = CreateMinimalState();
        // Pre-populate with existing transition properties
        state.Video.MixEffects[0].TransitionProperties.Style = TransitionStyle.Mix;
        state.Video.MixEffects[0].TransitionProperties.Selection = TransitionSelection.Background;
        state.Video.MixEffects[0].TransitionProperties.NextStyle = TransitionStyle.Mix;
        state.Video.MixEffects[0].TransitionProperties.NextSelection = TransitionSelection.Background;

        // Act
        command.ApplyToState(state);

        // Assert
        var mixEffect = state.Video.MixEffects[0];
        Assert.That(mixEffect.TransitionProperties, Is.Not.Null);
        Assert.That(mixEffect.TransitionProperties.Style, Is.EqualTo(TransitionStyle.Sting));
        Assert.That(mixEffect.TransitionProperties.Selection, Is.EqualTo(TransitionSelection.Key4));
        Assert.That(mixEffect.TransitionProperties.NextStyle, Is.EqualTo(TransitionStyle.Dip));
        Assert.That(mixEffect.TransitionProperties.NextSelection, Is.EqualTo(TransitionSelection.Background));
    }

    private static AtemState CreateMinimalState()
    {
        return new AtemState
        {
            Video = new VideoState
            {
                MixEffects =
                [
                    new MixEffect { Id = 0 },
                    new MixEffect { Id = 1 }
                ]
            }
        };
    }
}
