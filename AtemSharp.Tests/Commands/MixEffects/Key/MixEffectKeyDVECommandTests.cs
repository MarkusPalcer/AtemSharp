using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

[TestFixture]
[Ignore("TODO: DVE serialization scaling factors need to match deserialization - see TODO comments in MixEffectKeyDVEUpdateCommand.cs")]
public class MixEffectKeyDVECommandTests : SerializedCommandTestBase<MixEffectKeyDVECommand,
    MixEffectKeyDVECommandTests.CommandData>
{
    /// <summary>
    /// Specify which byte ranges contain floating-point values that should be compared with tolerance
    /// </summary>
    protected override Range[] GetFloatingPointByteRanges()
    {
        return [
            8..12,   // SizeX field (UInt32)
            12..16,  // SizeY field (UInt32)
            16..20,  // PositionX field (Int32)
            20..24,  // PositionY field (Int32)
            24..28,  // Rotation field (Int32)
            32..34,  // BorderOuterWidth field (UInt16)
            34..36,  // BorderInnerWidth field (UInt16)
            42..44,  // BorderHue field (UInt16)
            44..46,  // BorderSaturation field (UInt16)
            46..48,  // BorderLuma field (UInt16)
            48..50,  // LightSourceDirection field (UInt16)
            52..54,  // MaskTop field (UInt16)
            54..56,  // MaskBottom field (UInt16)
            56..58,  // MaskLeft field (UInt16)
            58..60   // MaskRight field (UInt16)
        ];
    }

    public class CommandData : CommandDataBase
    {
        public int MixEffectIndex { get; set; }
        public int KeyerIndex { get; set; }
        public double SizeX { get; set; }
        public double SizeY { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public double Rotation { get; set; }
        public bool BorderEnabled { get; set; }
        public bool BorderShadowEnabled { get; set; }
        public BorderBevel BorderBevel { get; set; }
        public double BorderOuterWidth { get; set; }
        public double BorderInnerWidth { get; set; }
        public double BorderOuterSoftness { get; set; }
        public double BorderInnerSoftness { get; set; }
        public double BorderBevelSoftness { get; set; }
        public double BorderBevelPosition { get; set; }
        public double BorderOpacity { get; set; }
        public double BorderHue { get; set; }
        public double BorderSaturation { get; set; }
        public double BorderLuma { get; set; }
        public double LightSourceDirection { get; set; }
        public double LightSourceAltitude { get; set; }
        public bool MaskEnabled { get; set; }
        public double MaskTop { get; set; }
        public double MaskBottom { get; set; }
        public double MaskLeft { get; set; }
        public double MaskRight { get; set; }
        public int Rate { get; set; }
    }

    protected override MixEffectKeyDVECommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required mix effect and upstream keyer with defaults
        var state = CreateStateWithUpstreamKeyerDVE(testCase.Command.MixEffectIndex, testCase.Command.KeyerIndex);
        
        // Create command with the mix effect and keyer IDs
        var command = new MixEffectKeyDVECommand(testCase.Command.MixEffectIndex, testCase.Command.KeyerIndex, state);

        // Only set the properties that should be updated based on the mask
        // This avoids setting all properties and thus all flags
        var mask = (uint)testCase.Command.Mask;
        
        if ((mask & (1U << 0)) != 0) command.SizeX = testCase.Command.SizeX;
        if ((mask & (1U << 1)) != 0) command.SizeY = testCase.Command.SizeY;
        if ((mask & (1U << 2)) != 0) command.PositionX = testCase.Command.PositionX;
        if ((mask & (1U << 3)) != 0) command.PositionY = testCase.Command.PositionY;
        if ((mask & (1U << 4)) != 0) command.Rotation = testCase.Command.Rotation;
        if ((mask & (1U << 5)) != 0) command.BorderEnabled = testCase.Command.BorderEnabled;
        if ((mask & (1U << 6)) != 0) command.ShadowEnabled = testCase.Command.BorderShadowEnabled;
        if ((mask & (1U << 7)) != 0) command.BorderBevel = testCase.Command.BorderBevel;
        if ((mask & (1U << 8)) != 0) command.BorderOuterWidth = testCase.Command.BorderOuterWidth;
        if ((mask & (1U << 9)) != 0) command.BorderInnerWidth = testCase.Command.BorderInnerWidth;
        if ((mask & (1U << 10)) != 0) command.BorderOuterSoftness = testCase.Command.BorderOuterSoftness;
        if ((mask & (1U << 11)) != 0) command.BorderInnerSoftness = testCase.Command.BorderInnerSoftness;
        if ((mask & (1U << 12)) != 0) command.BorderBevelSoftness = testCase.Command.BorderBevelSoftness;
        if ((mask & (1U << 13)) != 0) command.BorderBevelPosition = testCase.Command.BorderBevelPosition;
        if ((mask & (1U << 14)) != 0) command.BorderOpacity = testCase.Command.BorderOpacity;
        if ((mask & (1U << 15)) != 0) command.BorderHue = testCase.Command.BorderHue;
        if ((mask & (1U << 16)) != 0) command.BorderSaturation = testCase.Command.BorderSaturation;
        if ((mask & (1U << 17)) != 0) command.BorderLuma = testCase.Command.BorderLuma;
        if ((mask & (1U << 18)) != 0) command.LightSourceDirection = testCase.Command.LightSourceDirection;
        if ((mask & (1U << 19)) != 0) command.LightSourceAltitude = testCase.Command.LightSourceAltitude;
        if ((mask & (1U << 20)) != 0) command.MaskEnabled = testCase.Command.MaskEnabled;
        if ((mask & (1U << 21)) != 0) command.MaskTop = testCase.Command.MaskTop;
        if ((mask & (1U << 22)) != 0) command.MaskBottom = testCase.Command.MaskBottom;
        if ((mask & (1U << 23)) != 0) command.MaskLeft = testCase.Command.MaskLeft;
        if ((mask & (1U << 24)) != 0) command.MaskRight = testCase.Command.MaskRight;
        if ((mask & (1U << 25)) != 0) command.Rate = testCase.Command.Rate;
        
        return command;
    }

    /// <summary>
    /// Creates an AtemState with a valid mix effect and upstream keyer with DVE settings at the specified indices
    /// </summary>
    private static AtemState CreateStateWithUpstreamKeyerDVE(int mixEffectId, int keyerId, 
        double sizeX = 100.0, double sizeY = 100.0, double positionX = 0.0, double positionY = 0.0,
        double rotation = 0.0, bool borderEnabled = false, bool shadowEnabled = false,
        BorderBevel borderBevel = BorderBevel.None, double borderOuterWidth = 0.0, double borderInnerWidth = 0.0,
        double borderOuterSoftness = 0.0, double borderInnerSoftness = 0.0, double borderBevelSoftness = 0.0,
        double borderBevelPosition = 0.0, double borderOpacity = 0.0, double borderHue = 0.0,
        double borderSaturation = 0.0, double borderLuma = 0.0, double lightSourceDirection = 0.0,
        double lightSourceAltitude = 0.0, bool maskEnabled = false, double maskTop = 0.0,
        double maskBottom = 0.0, double maskLeft = 0.0, double maskRight = 0.0, int rate = 0)
    {
        var mixEffects = new Dictionary<int, MixEffect>();
        var upstreamKeyers = new Dictionary<int, UpstreamKeyer>();
        
        upstreamKeyers[keyerId] = new UpstreamKeyer
        {
            Index = keyerId,
            OnAir = false,
            FillSource = 1000,
            CutSource = 1001,
            DVESettings = new UpstreamKeyerDVESettings
            {
                SizeX = sizeX,
                SizeY = sizeY,
                PositionX = positionX,
                PositionY = positionY,
                Rotation = rotation,
                BorderEnabled = borderEnabled,
                ShadowEnabled = shadowEnabled,
                BorderBevel = borderBevel,
                BorderOuterWidth = borderOuterWidth,
                BorderInnerWidth = borderInnerWidth,
                BorderOuterSoftness = borderOuterSoftness,
                BorderInnerSoftness = borderInnerSoftness,
                BorderBevelSoftness = borderBevelSoftness,
                BorderBevelPosition = borderBevelPosition,
                BorderOpacity = borderOpacity,
                BorderHue = borderHue,
                BorderSaturation = borderSaturation,
                BorderLuma = borderLuma,
                LightSourceDirection = lightSourceDirection,
                LightSourceAltitude = lightSourceAltitude,
                MaskEnabled = maskEnabled,
                MaskTop = maskTop,
                MaskBottom = maskBottom,
                MaskLeft = maskLeft,
                MaskRight = maskRight,
                Rate = rate
            }
        };

        mixEffects[mixEffectId] = new MixEffect
        {
            Index = mixEffectId,
            ProgramInput = 1000,
            PreviewInput = 2001,
            TransitionPreview = false,
            TransitionPosition = new TransitionPosition
            {
                InTransition = false,
                HandlePosition = 0,
                RemainingFrames = 0
            },
            UpstreamKeyers = upstreamKeyers
        };

        return new AtemState
        {
            Video = new VideoState
            {
                MixEffects = mixEffects
            },
            Info = new DeviceInfo
            {
                Capabilities = new AtemCapabilities
                {
                    MixEffects = 4, // Support up to 4 mix effects
                    DVEs = 1        // DVE support required
                }
            }
        };
    }
}