using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class TransitionStingerCommandTests : SerializedCommandTestBase<TransitionStingerCommand,
    TransitionStingerCommandTests.CommandData>
{
    /// <inheritdoc />
    protected override Range[] GetFloatingPointByteRanges()
    {
        return [
            6..8,   // Clip
            8..10   // Gain
        ];
    }

    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public byte Source { get; set; }
        public bool PreMultipliedKey { get; set; }
        public double Clip { get; set; }
        public double Gain { get; set; }
        public bool Invert { get; set; }
        public ushort Preroll { get; set; }
        public ushort ClipDuration { get; set; }
        public ushort TriggerPoint { get; set; }
        public ushort MixRate { get; set; }
    }

    protected override TransitionStingerCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required mix effect and transition settings
        var state = new MixEffect
        {
            Index = testCase.Command.Index,
            TransitionSettings =
            {
                Stinger =
                {
                    Source = testCase.Command.Source,
                    PreMultipliedKey = testCase.Command.PreMultipliedKey,
                    Clip = testCase.Command.Clip,
                    Gain = testCase.Command.Gain,
                    Invert = testCase.Command.Invert,
                    Preroll = testCase.Command.Preroll,
                    ClipDuration = testCase.Command.ClipDuration,
                    TriggerPoint = testCase.Command.TriggerPoint,
                    MixRate = testCase.Command.MixRate
                }
            }
        };

        // Create command with the mix effect ID
        var command = new TransitionStingerCommand(state);

        // Set the actual values that should be written
        command.Source = testCase.Command.Source;
        command.PreMultipliedKey = testCase.Command.PreMultipliedKey;
        command.Clip = testCase.Command.Clip;
        command.Gain = testCase.Command.Gain;
        command.Invert = testCase.Command.Invert;
        command.Preroll = testCase.Command.Preroll;
        command.ClipDuration = testCase.Command.ClipDuration;
        command.TriggerPoint = testCase.Command.TriggerPoint;
        command.MixRate = testCase.Command.MixRate;

        return command;
    }

    [Test]
    public void Constructor_WithValidState_InitializesFromState()
    {
        // Arrange
        byte mixEffectId = 1;
        byte expectedSource = 3;
        var expectedPreMultipliedKey = true;
        var expectedClip = 25.5;
        var expectedGain = 50.0;
        var expectedInvert = true;
        ushort expectedPreroll = 1000;
        ushort expectedClipDuration = 2000;
        ushort expectedTriggerPoint = 1500;
        ushort expectedMixRate = 30;

        var state = new MixEffect
        {
            Index = mixEffectId,
            TransitionSettings =
            {
                Stinger =
                {
                    Source = expectedSource,
                    PreMultipliedKey = expectedPreMultipliedKey,
                    Clip = expectedClip,
                    Gain = expectedGain,
                    Invert = expectedInvert,
                    Preroll = expectedPreroll,
                    ClipDuration = expectedClipDuration,
                    TriggerPoint = expectedTriggerPoint,
                    MixRate = expectedMixRate
                }
            }
        };

        // Act
        var command = new TransitionStingerCommand(state);

        // Assert - Verify that values are correctly initialized from state
        Assert.That(command.MixEffectId, Is.EqualTo(mixEffectId));
        Assert.That(command.Source, Is.EqualTo(expectedSource));
        Assert.That(command.PreMultipliedKey, Is.EqualTo(expectedPreMultipliedKey));
        Assert.That(command.Clip, Is.EqualTo(expectedClip));
        Assert.That(command.Gain, Is.EqualTo(expectedGain));
        Assert.That(command.Invert, Is.EqualTo(expectedInvert));
        Assert.That(command.Preroll, Is.EqualTo(expectedPreroll));
        Assert.That(command.ClipDuration, Is.EqualTo(expectedClipDuration));
        Assert.That(command.TriggerPoint, Is.EqualTo(expectedTriggerPoint));
        Assert.That(command.MixRate, Is.EqualTo(expectedMixRate));

        // Verify that no flags are set initially (since we're loading from state)
        Assert.That(command.Flag, Is.EqualTo(0));
    }

    [Test]
    public void PropertyChanges_SetCorrectFlags()
    {
        // Arrange
        var state = new MixEffect();
        var command = new TransitionStingerCommand(state);

        // Act & Assert
        command.Source = 5;
        Assert.That(command.Flag & (1 << 0), Is.Not.EqualTo(0), "Source flag should be set");

        command.PreMultipliedKey = true;
        Assert.That(command.Flag & (1 << 1), Is.Not.EqualTo(0), "PreMultipliedKey flag should be set");

        command.Clip = 50.0;
        Assert.That(command.Flag & (1 << 2), Is.Not.EqualTo(0), "Clip flag should be set");

        command.Gain = 75.0;
        Assert.That(command.Flag & (1 << 3), Is.Not.EqualTo(0), "Gain flag should be set");

        command.Invert = true;
        Assert.That(command.Flag & (1 << 4), Is.Not.EqualTo(0), "Invert flag should be set");

        command.Preroll = 1000;
        Assert.That(command.Flag & (1 << 5), Is.Not.EqualTo(0), "Preroll flag should be set");

        command.ClipDuration = 2000;
        Assert.That(command.Flag & (1 << 6), Is.Not.EqualTo(0), "ClipDuration flag should be set");

        command.TriggerPoint = 1500;
        Assert.That(command.Flag & (1 << 7), Is.Not.EqualTo(0), "TriggerPoint flag should be set");

        command.MixRate = 30;
        Assert.That(command.Flag & (1 << 8), Is.Not.EqualTo(0), "MixRate flag should be set");
    }
}
