using System.Drawing;
using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

[Ignore("Verify correct byte sequence with ATEM Control software and compare with generated serialization")]
public class MixEffectKeyFlyKeyframeCommandTests : SerializedCommandTestBase<MixEffectKeyFlyKeyframeCommand, MixEffectKeyFlyKeyframeCommandTests.CommandData>
{
    protected override Range[] GetFloatingPointByteRanges() =>
    [
        (8..12),  // SizeX
        (12..16), // SizeY
        (16..20), // PositionX
        (20..24), // PositionY
        (24..28), // Rotation
        (28..30), // BorderOuterWidth
        (30..32), // BorderInnerWidth
        (38..40), // BorderHue
        (40..42), // BorderSaturation
        (42..44), // BorderLuma
        (44..46), // LightSourceDirection
        (48..50), // MaskTop
        (50..52), // MaskBottom
        (52..54), // MaskLeft
        (54..56)  // MaskRight
    ];

    public class CommandData : CommandDataBase
    {
        public byte MixEffectIndex  { get; set; }
        public byte KeyerIndex  { get; set; }
        public byte KeyFrame { get; set; }
        public double SizeX { get; set; }
        public double SizeY { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public double Rotation { get; set; }
        public double OuterWidth { get; set; }
        public double InnerWidth { get; set; }
        public byte OuterSoftness { get; set; }
        public byte InnerSoftness { get; set; }
        public byte BevelSoftness { get; set; }
        public byte BevelPosition { get; set; }
        public byte BorderOpacity  { get; set; }
        public ushort BorderHue { get; set; }
        public ushort BorderSaturation { get; set; }
        public ushort BorderLuma { get; set; }
        public double LightSourceDirection { get; set; }
        public byte LightSourceAltitude { get; set; }
        public double MaskTop { get; set; }
        public double MaskBottom { get; set; }
        public double MaskLeft { get; set; }
        public double MaskRight { get; set; }
    }

    protected override MixEffectKeyFlyKeyframeCommand CreateSut(TestCaseData testCase)
    {
        return new MixEffectKeyFlyKeyframeCommand(new UpstreamKeyerFlyKeyframe
        {
            UpstreamKeyerId = testCase.Command.KeyerIndex,
            MixEffectId = testCase.Command.MixEffectIndex,
            Id = testCase.Command.KeyFrame,
            Size = new SizeF((float)testCase.Command.SizeX, (float)testCase.Command.SizeY),
            Location = new PointF((float) testCase.Command.PositionX, (float)testCase.Command.PositionY),
            Rotation = testCase.Command.Rotation,
            Border =
            {
                BevelPosition = testCase.Command.BevelPosition,
                BevelSoftness = testCase.Command.BevelSoftness,
                Hue = testCase.Command.BorderHue,
                Saturation = testCase.Command.BorderSaturation,
                Luma = testCase.Command.BorderLuma,
                InnerSoftness = testCase.Command.InnerSoftness,
                InnerWidth = testCase.Command.InnerWidth,
                Opacity = testCase.Command.BorderOpacity,
                OuterSoftness = testCase.Command.OuterSoftness,
                OuterWidth = testCase.Command.OuterWidth
            },
            LightSourceDirection = testCase.Command.LightSourceDirection,
            LightSourceAltitude = testCase.Command.LightSourceAltitude,
            Mask =
            {
                Top = testCase.Command.MaskTop,
                Bottom = testCase.Command.MaskBottom,
                Left = testCase.Command.MaskLeft,
                Right = testCase.Command.MaskRight
            }
        });
    }
}
