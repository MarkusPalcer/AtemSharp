using AtemSharp.Commands.SuperSource;
using AtemSharp.State.Border;
using AtemSharp.State.Info;
using AtemSharp.State.Video.SuperSource;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Commands.SuperSource;

public class SuperSourcePropertiesCommandTests : SerializedCommandTestBase<SuperSourcePropertiesCommand,
    SuperSourcePropertiesCommandTests.CommandData>
{
    protected override Range[] GetFloatingPointByteRanges() =>
    [
        (10..12),
        (12..14),
        (18..20),
        (20..22),
        (26..28),
        (28..30),
        (30..32),
        (32..34),
        (34..35)
    ];

    [MaxProtocolVersion(ProtocolVersion.V7_5_2)]
    public class CommandData : CommandDataBase
    {
        public ushort ArtFillSource { get; set; }
        public ushort ArtCutSource { get; set; }
        public ArtOption ArtOption { get; set; }
        public bool ArtPreMultiplied { get; set; }
        public double ArtClip { get; set; }
        public double ArtGain { get; set; }
        public bool ArtInvertKey { get; set; }
        public bool BorderEnabled { get; set; }
        public BorderBevel BorderBevel { get; set; }
        public double BorderOuterWidth { get; set; }
        public double BorderInnerWidth { get; set; }
        public byte BorderOuterSoftness { get; set; }
        public byte BorderInnerSoftness { get; set; }
        public byte BorderBevelSoftness { get; set; }
        public byte BorderBevelPosition { get; set; }
        public double BorderHue { get; set; }
        public double BorderSaturation { get; set; }
        public double BorderLuma { get; set; }
        public double BorderLightSourceDirection { get; set; }
        public double BorderLightSourceAltitude { get; set; }
    }

    protected override SuperSourcePropertiesCommand CreateSut(TestCaseData testCase)
    {
        return new SuperSourcePropertiesCommand(new AtemSharp.State.Video.SuperSource.SuperSource
        {
            FillSource = testCase.Command.ArtFillSource,
            CutSource = testCase.Command.ArtCutSource,
            Option = testCase.Command.ArtOption,
            PreMultiplied = testCase.Command.ArtPreMultiplied,
            Clip = testCase.Command.ArtClip,
            Gain = testCase.Command.ArtGain,
            InvertKey = testCase.Command.ArtInvertKey,
            Border =
            {
                Enabled = testCase.Command.BorderEnabled,
                Bevel = testCase.Command.BorderBevel,
                OuterWidth = testCase.Command.BorderOuterWidth,
                InnerWidth = testCase.Command.BorderInnerWidth,
                OuterSoftness = testCase.Command.BorderOuterSoftness,
                InnerSoftness = testCase.Command.BorderInnerSoftness,
                BevelSoftness = testCase.Command.BorderBevelSoftness,
                BevelPosition = testCase.Command.BorderBevelPosition,
                Hue = testCase.Command.BorderHue,
                Saturation = testCase.Command.BorderSaturation,
                Luma = testCase.Command.BorderLuma,
                LightSourceDirection = testCase.Command.BorderLightSourceDirection,
                LightSourceAltitude = testCase.Command.BorderLightSourceAltitude
            }
        });
    }
}
