using System.Drawing;
using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.State.Video.MixEffect.UpstreamKeyer;
using AtemSharp.Types;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

// TODO #82: Capture real test data and verify implementation and tests
[Ignore("Verify correct byte sequence with ATEM Control software and compare with generated serialization")]
public class MixEffectKeyFlyKeyframeCommandTests : SerializedCommandTestBase<MixEffectKeyFlyKeyframeCommand,
    MixEffectKeyFlyKeyframeCommandTests.CommandData>
{
    protected override Range[] GetFloatingPointByteRanges() =>
    [
        (8..12), // SizeX
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
        (54..56) // MaskRight
    ];

    public class CommandData : CommandDataBase
    {
        public byte MixEffectIndex { get; set; }
        public byte KeyerIndex { get; set; }
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
        public byte BorderOpacity { get; set; }
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
            Location = new PointF((float)testCase.Command.PositionX, (float)testCase.Command.PositionY),
            Rotation = testCase.Command.Rotation,
            Border =
            {
                BevelPosition = testCase.Command.BevelPosition,
                BevelSoftness = testCase.Command.BevelSoftness,
                Color = new HslColor(
                    testCase.Command.BorderHue,
                    testCase.Command.BorderSaturation,
                    testCase.Command.BorderLuma),
                InnerSoftness = testCase.Command.InnerSoftness,
                InnerWidth = testCase.Command.InnerWidth,
                Opacity = testCase.Command.BorderOpacity,
                OuterSoftness = testCase.Command.OuterSoftness,
                OuterWidth = testCase.Command.OuterWidth
            },
            Shadow = {
            LightSourceDirection = testCase.Command.LightSourceDirection,
            LightSourceAltitude = testCase.Command.LightSourceAltitude,
            },
            Mask =
            {
                Top = testCase.Command.MaskTop,
                Bottom = testCase.Command.MaskBottom,
                Left = testCase.Command.MaskLeft,
                Right = testCase.Command.MaskRight
            }
        });
    }


    [Test]
    public void SettingLocation_ShouldSetPositionXAndPositionY()
    {
        var sut = new MixEffectKeyFlyKeyframeCommand(new())
        {
            Location = new PointF((float)12.3, (float)45.6)
        };

        Assert.Multiple(() =>
        {
            Assert.That(sut.PositionX, Is.EqualTo(12.3).Within(0.01));
            Assert.That(sut.PositionY, Is.EqualTo(45.6).Within(0.01));
        });
    }

    [Test]
    public void BettingLocation_ShouldGetPositionXAndPositionY()
    {
        var sut = new MixEffectKeyFlyKeyframeCommand(new())
        {
            PositionX = 12.3,
            PositionY = 45.6
        };

        Assert.Multiple(() =>
        {
            Assert.That(sut.Location.X, Is.EqualTo(12.3).Within(0.01));
            Assert.That(sut.Location.Y, Is.EqualTo(45.6).Within(0.01));
        });
    }

    [Test]
    public void GettingSize_ShouldGetWidthAndHeight()
    {
        var sut = new MixEffectKeyFlyKeyframeCommand(new())
        {
            SizeX = 12.3,
            SizeY = 45.6
        };

        Assert.Multiple(() =>
        {
            Assert.That(sut.Size.Width, Is.EqualTo(12.3).Within(0.01));
            Assert.That(sut.Size.Height, Is.EqualTo(45.6).Within(0.01));
        });
    }

    [Test]
    public void SettingSize_ShouldSetWidthAndHeight()
    {
        var sut = new MixEffectKeyFlyKeyframeCommand(new())
        {
            Size = new SizeF(12.3f, 45.6f)
        };

        Assert.Multiple(() =>
        {
            Assert.That(sut.SizeX, Is.EqualTo(12.3).Within(0.01));
            Assert.That(sut.SizeY, Is.EqualTo(45.6).Within(0.01));
        });
    }

    [Test]
    public void GettingBounds_ShouldGetLocationAndSize()
    {
        var sut = new MixEffectKeyFlyKeyframeCommand(new ())
        {
            Location = new PointF(12.3f, 45.6f),
            Size = new SizeF(78.9f, 0.12f),
        };

        Assert.Multiple(() =>
        {
            Assert.That(sut.Bounds.Location.X, Is.EqualTo(12.3f).Within(0.01));
            Assert.That(sut.Bounds.Location.Y, Is.EqualTo(45.6f).Within(0.01));
            Assert.That(sut.Bounds.Size.Width, Is.EqualTo(78.9f).Within(0.01));
            Assert.That(sut.Bounds.Size.Height, Is.EqualTo(0.12f).Within(0.01));
        });
    }

    [Test]
    public void SettingBounds_ShouldSetLocationAndSize()
    {
        var sut = new MixEffectKeyFlyKeyframeCommand(new())
        {
            Bounds = new RectangleF(12.3f, 45.6f, 78.9f, 0.12f)
        };

        Assert.Multiple(() =>
        {
            Assert.That(sut.Location.X, Is.EqualTo(12.3f).Within(0.01));
            Assert.That(sut.Location.Y, Is.EqualTo(45.6f).Within(0.01));
            Assert.That(sut.SizeX, Is.EqualTo(78.9).Within(0.01));
            Assert.That(sut.SizeY, Is.EqualTo(0.12f).Within(0.01));
        });
    }
}
