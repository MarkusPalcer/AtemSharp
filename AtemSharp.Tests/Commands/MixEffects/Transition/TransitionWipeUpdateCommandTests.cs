using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class TransitionWipeUpdateCommandTests : DeserializedCommandTestBase<TransitionWipeUpdateCommand,
    TransitionWipeUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public byte Rate { get; set; }
        public byte Pattern { get; set; }
        public double BorderWidth { get; set; }
        public ushort BorderInput { get; set; }
        public double Symmetry { get; set; }
        public double BorderSoftness { get; set; }
        public double XPosition { get; set; }
        public double YPosition { get; set; }
        public bool ReverseDirection { get; set; }
        public bool FlipFlop { get; set; }
    }

    protected override void CompareCommandProperties(TransitionWipeUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(testCase.Command.Index));
        Assert.That(actualCommand.Rate, Is.EqualTo(expectedData.Rate));
        Assert.That(actualCommand.Pattern, Is.EqualTo(expectedData.Pattern));
        Assert.That(actualCommand.BorderWidth, Is.EqualTo(expectedData.BorderWidth).Within(0.01));
        Assert.That(actualCommand.BorderInput, Is.EqualTo(expectedData.BorderInput));
        Assert.That(actualCommand.Symmetry, Is.EqualTo(expectedData.Symmetry).Within(0.01));
        Assert.That(actualCommand.BorderSoftness, Is.EqualTo(expectedData.BorderSoftness).Within(0.01));
        Assert.That(actualCommand.XPosition, Is.EqualTo(expectedData.XPosition).Within(0.0001));
        Assert.That(actualCommand.YPosition, Is.EqualTo(expectedData.YPosition).Within(0.0001));
        Assert.That(actualCommand.ReverseDirection, Is.EqualTo(expectedData.ReverseDirection));
        Assert.That(actualCommand.FlipFlop, Is.EqualTo(expectedData.FlipFlop));
    }

    [Test]
    public void ApplyToState_ValidState_UpdatesWipeSettings()
    {
        // Arrange
        var command = new TransitionWipeUpdateCommand
        {
            MixEffectId = 0,
            Rate = 75,
            Pattern = 5,
            BorderWidth = 12.5,
            BorderInput = 2048,
            Symmetry = 65.3,
            BorderSoftness = 33.7,
            XPosition = 0.7856,
            YPosition = 0.2341,
            ReverseDirection = true,
            FlipFlop = false
        };

        var state = new AtemState
        {
            Info = new DeviceInfo
            {
                Capabilities = new AtemCapabilities
                {
                    MixEffects = 1
                }
            },
            Video = new VideoState
            {
                MixEffects =
                [
                    new MixEffect
                    {
                        Id = 0,
                        ProgramInput = 1000,
                        PreviewInput = 1001
                    }
                ]
            }
        };

        // Act
        command.ApplyToState(state);

        // Assert
        var mixEffect = state.Video.MixEffects[0];
        Assert.That(mixEffect.TransitionSettings, Is.Not.Null);
        Assert.That(mixEffect.TransitionSettings.Wipe, Is.Not.Null);

        var wipeSettings = mixEffect.TransitionSettings.Wipe;
        Assert.That(wipeSettings.Rate, Is.EqualTo(75));
        Assert.That(wipeSettings.Pattern, Is.EqualTo(5));
        Assert.That(wipeSettings.BorderWidth, Is.EqualTo(12.5));
        Assert.That(wipeSettings.BorderInput, Is.EqualTo(2048));
        Assert.That(wipeSettings.Symmetry, Is.EqualTo(65.3));
        Assert.That(wipeSettings.BorderSoftness, Is.EqualTo(33.7));
        Assert.That(wipeSettings.XPosition, Is.EqualTo(0.7856));
        Assert.That(wipeSettings.YPosition, Is.EqualTo(0.2341));
        Assert.That(wipeSettings.ReverseDirection, Is.True);
        Assert.That(wipeSettings.FlipFlop, Is.False);
    }
}
