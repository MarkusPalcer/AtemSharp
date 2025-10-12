using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
// ReSharper disable once InconsistentNaming Domain Specific Acronym
public class TransitionDVECommandTests : SerializedCommandTestBase<TransitionDVECommand,
    TransitionDVECommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public int Rate { get; set; }
        public int LogoRate { get; set; }
        public DVEEffect Style { get; set; }
        public int FillSource { get; set; }
        public int KeySource { get; set; }
        public bool EnableKey { get; set; }
        public bool PreMultiplied { get; set; }
        public double Clip { get; set; }  // Use double for fractional values
        public double Gain { get; set; }  // Use double for fractional values
        public bool InvertKey { get; set; }
        public bool Reverse { get; set; }
        public bool FlipFlop { get; set; }
    }

    protected override TransitionDVECommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required mix effect and transition settings
        var state = CreateStateWithMixEffect(testCase.Command.Index, 
            testCase.Command.Rate, testCase.Command.LogoRate, testCase.Command.Style,
            testCase.Command.FillSource, testCase.Command.KeySource, testCase.Command.EnableKey,
            testCase.Command.PreMultiplied, testCase.Command.Clip, testCase.Command.Gain,
            testCase.Command.InvertKey, testCase.Command.Reverse, testCase.Command.FlipFlop);
        
        // Create command with the mix effect ID
        var command = new TransitionDVECommand(testCase.Command.Index, state);

        // Set the actual values that should be written
        command.Rate = testCase.Command.Rate;
        command.LogoRate = testCase.Command.LogoRate;
        command.Style = testCase.Command.Style;
        command.FillSource = testCase.Command.FillSource;
        command.KeySource = testCase.Command.KeySource;
        command.EnableKey = testCase.Command.EnableKey;
        command.PreMultiplied = testCase.Command.PreMultiplied;
        command.Clip = testCase.Command.Clip;  // Direct assignment - now both are double
        command.Gain = testCase.Command.Gain;  // Direct assignment - now both are double
        command.InvertKey = testCase.Command.InvertKey;
        command.Reverse = testCase.Command.Reverse;
        command.FlipFlop = testCase.Command.FlipFlop;
        
        return command;
    }

    /// <summary>
    /// Creates an AtemState with a valid mix effect and DVE transition settings at the specified index
    /// </summary>
    private static AtemState CreateStateWithMixEffect(int mixEffectId, int rate = 0, int logoRate = 0, 
        DVEEffect style = DVEEffect.SwooshTopLeft, int fillSource = 0, int keySource = 0, 
        bool enableKey = false, bool preMultiplied = false, double clip = 0.0, double gain = 0.0,
        bool invertKey = false, bool reverse = false, bool flipFlop = false)
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
                DVE = new DVETransitionSettings
                {
                    Rate = rate,
                    LogoRate = logoRate,
                    Style = style,
                    FillSource = fillSource,
                    KeySource = keySource,
                    EnableKey = enableKey,
                    PreMultiplied = preMultiplied,
                    Clip = clip,
                    Gain = gain,
                    InvertKey = invertKey,
                    Reverse = reverse,
                    FlipFlop = flipFlop
                }
            }
        };
        
        return new AtemState
        {
            Info = new DeviceInfo
            {
                Capabilities = new AtemCapabilities
                {
                    MixEffects = mixEffectId + 1,
                    DVEs = 1
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
        var expectedRate = 50;
        var expectedLogoRate = 30;
        var expectedStyle = DVEEffect.SwooshTop;
        var expectedFillSource = 1000;
        var expectedKeySource = 2000;
        var state = CreateStateWithMixEffect(0, expectedRate, expectedLogoRate, expectedStyle, 
            expectedFillSource, expectedKeySource, true, true, 500, 750, true, true, true);

        // Act
        var command = new TransitionDVECommand(0, state);

        // Assert
        Assert.That(command.MixEffectId, Is.EqualTo(0));
        Assert.That(command.Rate, Is.EqualTo(expectedRate));
        Assert.That(command.LogoRate, Is.EqualTo(expectedLogoRate));
        Assert.That(command.Style, Is.EqualTo(expectedStyle));
        Assert.That(command.FillSource, Is.EqualTo(expectedFillSource));
        Assert.That(command.KeySource, Is.EqualTo(expectedKeySource));
        Assert.That(command.EnableKey, Is.True);
        Assert.That(command.PreMultiplied, Is.True);
        Assert.That(command.Clip, Is.EqualTo(500));
        Assert.That(command.Gain, Is.EqualTo(750));
        Assert.That(command.InvertKey, Is.True);
        Assert.That(command.Reverse, Is.True);
        Assert.That(command.FlipFlop, Is.True);
        
        // No flags should be set since we initialized from state
        Assert.That(command.Flag, Is.EqualTo(0));
    }

    [Test]
    public void Constructor_WithNullDVESettings_InitializesWithDefaults()
    {
        // Arrange
        var state = new AtemState
        {
            Info = new DeviceInfo
            {
                Capabilities = new AtemCapabilities { MixEffects = 1, DVEs = 1 }
            },
            Video = new VideoState { MixEffects = new Dictionary<int, MixEffect>() }
        };

        // Act
        var command = new TransitionDVECommand(0, state);

        // Assert
        Assert.That(command.MixEffectId, Is.EqualTo(0));
        Assert.That(command.Rate, Is.EqualTo(0));
        Assert.That(command.LogoRate, Is.EqualTo(0));
        Assert.That(command.Style, Is.EqualTo(DVEEffect.SwooshTopLeft));
        Assert.That(command.FillSource, Is.EqualTo(0));
        Assert.That(command.KeySource, Is.EqualTo(0));
        Assert.That(command.EnableKey, Is.False);
        Assert.That(command.PreMultiplied, Is.False);
        Assert.That(command.Clip, Is.EqualTo(0));
        Assert.That(command.Gain, Is.EqualTo(0));
        Assert.That(command.InvertKey, Is.False);
        Assert.That(command.Reverse, Is.False);
        Assert.That(command.FlipFlop, Is.False);
        
        // All flags should be set since we set defaults
        Assert.That(command.Flag, Is.EqualTo((1 << 12) - 1)); // All 12 flags set
    }

    [Test]
    public void PropertySetters_SetCorrectFlags()
    {
        // Arrange
        var state = CreateStateWithMixEffect(0);
        var command = new TransitionDVECommand(0, state);
        
        // Act & Assert individual flag setting
        command.Rate = 25;
        Assert.That(command.Flag & (1 << 0), Is.Not.EqualTo(0), "Rate flag not set");
        
        command.LogoRate = 15;
        Assert.That(command.Flag & (1 << 1), Is.Not.EqualTo(0), "LogoRate flag not set");
        
        command.Style = DVEEffect.SwooshRight;
        Assert.That(command.Flag & (1 << 2), Is.Not.EqualTo(0), "Style flag not set");
        
        command.FillSource = 1000;
        Assert.That(command.Flag & (1 << 3), Is.Not.EqualTo(0), "FillSource flag not set");
        
        command.KeySource = 2000;
        Assert.That(command.Flag & (1 << 4), Is.Not.EqualTo(0), "KeySource flag not set");
        
        command.EnableKey = true;
        Assert.That(command.Flag & (1 << 5), Is.Not.EqualTo(0), "EnableKey flag not set");
        
        command.PreMultiplied = true;
        Assert.That(command.Flag & (1 << 6), Is.Not.EqualTo(0), "PreMultiplied flag not set");
        
        command.Clip = 500;
        Assert.That(command.Flag & (1 << 7), Is.Not.EqualTo(0), "Clip flag not set");
        
        command.Gain = 750;
        Assert.That(command.Flag & (1 << 8), Is.Not.EqualTo(0), "Gain flag not set");
        
        command.InvertKey = true;
        Assert.That(command.Flag & (1 << 9), Is.Not.EqualTo(0), "InvertKey flag not set");
        
        command.Reverse = true;
        Assert.That(command.Flag & (1 << 10), Is.Not.EqualTo(0), "Reverse flag not set");
        
        command.FlipFlop = true;
        Assert.That(command.Flag & (1 << 11), Is.Not.EqualTo(0), "FlipFlop flag not set");
    }
}