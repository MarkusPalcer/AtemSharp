using System.Drawing;
using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.Types.Border;
using AtemSharp.State.Video.MixEffect.UpstreamKeyer;
using AtemSharp.Types;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

// TODO #80: Capture test data and use test class base
//           for some reason byte 31 contains data in the TS test data and I don't know which
[Ignore("TODO #80: Capture test data and use test class base")]
public class MixEffectKeyDigitalVideoEffectsCommandTests : SerializedCommandTestBase<MixEffectKeyDigitalVideoEffectsCommand,
    MixEffectKeyDigitalVideoEffectsCommandTests.CommandData>
{
    /// <summary>
    /// Specify which byte ranges contain floating-point values that should be compared with tolerance
    /// </summary>
    protected override Range[] GetFloatingPointByteRanges()
    {
        return
        [
            8..12, // SizeX field (UInt32)
            12..16, // SizeY field (UInt32)
            16..20, // PositionX field (Int32)
            20..24, // PositionY field (Int32)
            24..28, // Rotation field (Int32)
            32..34, // BorderOuterWidth field (UInt16)
            34..36, // BorderInnerWidth field (UInt16)
            42..44, // BorderHue field (UInt16)
            44..46, // BorderSaturation field (UInt16)
            46..48, // BorderLuma field (UInt16)
            48..50, // LightSourceDirection field (UInt16)
            52..54, // MaskTop field (UInt16)
            54..56, // MaskBottom field (UInt16)
            56..58, // MaskLeft field (UInt16)
            58..60 // MaskRight field (UInt16)
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
        public byte BorderOuterSoftness { get; set; }
        public byte BorderInnerSoftness { get; set; }
        public byte BorderBevelSoftness { get; set; }
        public byte BorderBevelPosition { get; set; }
        public byte BorderOpacity { get; set; }
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
                Border =
                {
                    Bevel = testCase.Command.BorderBevel,
                    BevelPosition = testCase.Command.BorderBevelPosition,
                    BevelSoftness = testCase.Command.BorderBevelSoftness,
                    Enabled = testCase.Command.BorderEnabled,
                    InnerSoftness = testCase.Command.BorderInnerSoftness,
                    OuterSoftness = testCase.Command.BorderOuterSoftness,
                    Color = new HslColor(
                        testCase.Command.BorderHue,
                        testCase.Command.BorderSaturation,
                        testCase.Command.BorderLuma),
                    Opacity = testCase.Command.BorderOpacity,
                    OuterWidth = testCase.Command.BorderOuterWidth,
                    InnerWidth = testCase.Command.BorderInnerWidth,
                },
                Shadow =
                {
                    LightSourceAltitude = testCase.Command.LightSourceAltitude,
                    LightSourceDirection = testCase.Command.LightSourceDirection,
                    Enabled = testCase.Command.BorderShadowEnabled,
                },
                Rate = testCase.Command.Rate,
                Size = new SizeF(
                    (float)testCase.Command.SizeX,
                    (float)testCase.Command.SizeY),
                Location = new PointF(
                    (float)testCase.Command.PositionX,
                    (float)testCase.Command.PositionY),
                Rotation = testCase.Command.Rotation,
                Mask =
                {
                    Enabled = testCase.Command.MaskEnabled,
                    Top = testCase.Command.MaskTop,
                    Bottom = testCase.Command.MaskBottom,
                    Left = testCase.Command.MaskLeft,
                    Right = testCase.Command.MaskRight
                }
            }
        });
    }


    [Test]
    public void SettingLocation_ShouldSetPositionXAndPositionY()
    {
        var state = new UpstreamKeyer();
        var sut = new MixEffectKeyDigitalVideoEffectsCommand(state)
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
    public void GettingLocation_ShouldGetPositionXAndPositionY()
    {
        var state = new UpstreamKeyer();
        var sut = new MixEffectKeyDigitalVideoEffectsCommand(state)
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
        var sut = new MixEffectKeyDigitalVideoEffectsCommand(new UpstreamKeyer())
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
        var sut = new MixEffectKeyDigitalVideoEffectsCommand(new UpstreamKeyer())
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
        var sut = new MixEffectKeyDigitalVideoEffectsCommand(new UpstreamKeyer())
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
        var sut = new MixEffectKeyDigitalVideoEffectsCommand(new UpstreamKeyer())
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
