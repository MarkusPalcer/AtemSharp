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
        public int Source { get; set; }
        public bool PreMultipliedKey { get; set; }
        public double Clip { get; set; }
        public double Gain { get; set; }
        public bool Invert { get; set; }
        public int Preroll { get; set; }
        public int ClipDuration { get; set; }
        public int TriggerPoint { get; set; }
        public int MixRate { get; set; }
    }

    protected override TransitionStingerCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required mix effect and transition settings
        var state = CreateStateWithMixEffect(testCase.Command.Index, 
            testCase.Command.Source, testCase.Command.PreMultipliedKey,
            testCase.Command.Clip, testCase.Command.Gain,
            testCase.Command.Invert, testCase.Command.Preroll,
            testCase.Command.ClipDuration, testCase.Command.TriggerPoint,
            testCase.Command.MixRate);
        
        // Create command with the mix effect ID
        var command = new TransitionStingerCommand(testCase.Command.Index, state);

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

    /// <summary>
    /// Creates an AtemState with a valid mix effect and stinger transition settings at the specified index
    /// </summary>
    private static AtemState CreateStateWithMixEffect(int mixEffectId, int source = 0, bool preMultipliedKey = false,
        double clip = 0.0, double gain = 0.0, bool invert = false, int preroll = 0,
        int clipDuration = 0, int triggerPoint = 0, int mixRate = 0)
    {
        Dictionary<int, MixEffect> mixEffects = new Dictionary<int, MixEffect>();
        mixEffects[mixEffectId] = new MixEffect
        {
            Index = mixEffectId,
            ProgramInput = 1000,
            PreviewInput = 1001,
            TransitionPreview = false,
            TransitionPosition = new TransitionPosition
            {
                InTransition = false,
                RemainingFrames = 0,
                HandlePosition = 0.0
            },
            TransitionSettings = new TransitionSettings
            {
                Stinger = new StingerTransitionSettings
                {
                    Source = source,
                    PreMultipliedKey = preMultipliedKey,
                    Clip = clip,
                    Gain = gain,
                    Invert = invert,
                    Preroll = preroll,
                    ClipDuration = clipDuration,
                    TriggerPoint = triggerPoint,
                    MixRate = mixRate
                }
            }
        };
        
        return new AtemState
        {
            Info = new DeviceInfo
            {
                Capabilities = new AtemCapabilities
                {
                    MixEffects = mixEffectId + 1
                }
            },
            Video = new VideoState
            {
                MixEffects = mixEffects
            }
        };
    }

    [Test]
    public void Constructor_WithValidState_InitializesFromState()
    {
        // Arrange
        var mixEffectId = 1;
        var expectedSource = 3;
        var expectedPreMultipliedKey = true;
        var expectedClip = 25.5;
        var expectedGain = 50.0;
        var expectedInvert = true;
        var expectedPreroll = 1000;
        var expectedClipDuration = 2000;
        var expectedTriggerPoint = 1500;
        var expectedMixRate = 30;

        var state = CreateStateWithMixEffect(mixEffectId, expectedSource, expectedPreMultipliedKey,
            expectedClip, expectedGain, expectedInvert, expectedPreroll,
            expectedClipDuration, expectedTriggerPoint, expectedMixRate);

        // Act
        var command = new TransitionStingerCommand(mixEffectId, state);

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
    public void Constructor_WithMissingState_InitializesWithDefaults()
    {
        // Arrange
        var mixEffectId = 0;
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
                MixEffects = new Dictionary<int, MixEffect>()
            }
        };

        // Act
        var command = new TransitionStingerCommand(mixEffectId, state);

        // Assert - Verify that default values are set and all flags are set
        Assert.That(command.MixEffectId, Is.EqualTo(mixEffectId));
        Assert.That(command.Source, Is.EqualTo(0));
        Assert.That(command.PreMultipliedKey, Is.EqualTo(false));
        Assert.That(command.Clip, Is.EqualTo(0));
        Assert.That(command.Gain, Is.EqualTo(0));
        Assert.That(command.Invert, Is.EqualTo(false));
        Assert.That(command.Preroll, Is.EqualTo(0));
        Assert.That(command.ClipDuration, Is.EqualTo(0));
        Assert.That(command.TriggerPoint, Is.EqualTo(0));
        Assert.That(command.MixRate, Is.EqualTo(0));

        // Verify that all flags are set (since we're setting defaults)
        Assert.That(command.Flag, Is.EqualTo(0x1FF)); // All 9 flags set
    }

    [Test]
    public void PropertyChanges_SetCorrectFlags()
    {
        // Arrange
        var state = CreateStateWithMixEffect(0);
        var command = new TransitionStingerCommand(0, state);

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