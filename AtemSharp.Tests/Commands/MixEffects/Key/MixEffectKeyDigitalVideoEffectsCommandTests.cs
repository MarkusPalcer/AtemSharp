using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

[TestFixture]
[Ignore("TODO: DVE serialization scaling factors need to match deserialization - see TODO comments in MixEffectKeyDVEUpdateCommand.cs")]
public class MixEffectKeyDigitalVideoEffectsCommandTests : SerializedCommandTestBase<MixEffectKeyDigitalVideoEffectsCommand,
    MixEffectKeyDigitalVideoEffectsCommandTests.CommandData>
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
        public byte MixEffectIndex { get; set; }
        public byte KeyerIndex { get; set; }
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
        public byte Rate { get; set; }
    }

    protected override MixEffectKeyDigitalVideoEffectsCommand CreateSut(TestCaseData testCase)
    {
        // Create command with the mix effect and keyer IDs
        return new MixEffectKeyDigitalVideoEffectsCommand(new UpstreamKeyer
        {
            MixEffectId = testCase.Command.MixEffectIndex,
            Id = testCase.Command.KeyerIndex,
            DigitalVideoEffectsSettings =
            {
                BorderBevel = testCase.Command.BorderBevel,
                BorderBevelPosition = testCase.Command.BorderBevelPosition,
                BorderBevelSoftness = testCase.Command.BorderBevelSoftness,
                LightSourceAltitude = testCase.Command.LightSourceAltitude,
                BorderEnabled = testCase.Command.BorderEnabled,
                ShadowEnabled = testCase.Command.BorderShadowEnabled,
                BorderInnerSoftness = testCase.Command.BorderInnerSoftness,
                BorderOuterSoftness = testCase.Command.BorderOuterSoftness,
                BorderHue = testCase.Command.BorderHue,
                BorderSaturation = testCase.Command.BorderSaturation,
                BorderLuma = testCase.Command.BorderLuma,
                BorderOpacity = testCase.Command.BorderOpacity,
                Rate = testCase.Command.Rate,
                SizeX = testCase.Command.SizeX,
                SizeY = testCase.Command.SizeY,
                PositionX = testCase.Command.PositionX,
                PositionY = testCase.Command.PositionY,
                Rotation = testCase.Command.Rotation,
                BorderOuterWidth = testCase.Command.BorderOuterWidth,
                BorderInnerWidth = testCase.Command.BorderInnerWidth,
                LightSourceDirection = testCase.Command.LightSourceDirection,
                MaskEnabled = testCase.Command.MaskEnabled,
                MaskTop = testCase.Command.MaskTop,
                MaskBottom = testCase.Command.MaskBottom,
                MaskLeft = testCase.Command.MaskLeft,
                MaskRight = testCase.Command.MaskRight
            }
        });
    }
}
