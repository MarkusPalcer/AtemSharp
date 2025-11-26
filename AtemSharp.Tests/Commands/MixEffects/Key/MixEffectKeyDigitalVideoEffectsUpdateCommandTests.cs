using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.State.Border;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

[TestFixture]
[Ignore("TODO: DVE scaling factors need refinement")]
public class MixEffectKeyDigitalVideoEffectsUpdateCommandTests : DeserializedCommandTestBase<MixEffectKeyDigitalVideoEffectsUpdateCommand,
    MixEffectKeyDigitalVideoEffectsUpdateCommandTests.CommandData>
{
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

    protected override void CompareCommandProperties(MixEffectKeyDigitalVideoEffectsUpdateCommand command, CommandData expected, TestCaseData testCase)
    {
        Assert.That(command.MixEffectId, Is.EqualTo(expected.MixEffectIndex));
        Assert.That(command.KeyerId, Is.EqualTo(expected.KeyerIndex));
        Assert.That(command.SizeX, Is.EqualTo(expected.SizeX));
        Assert.That(command.SizeY, Is.EqualTo(expected.SizeY));
        Assert.That(command.PositionX, Is.EqualTo(expected.PositionX));
        Assert.That(command.PositionY, Is.EqualTo(expected.PositionY));
        Assert.That(command.Rotation, Is.EqualTo(expected.Rotation));
        Assert.That(command.BorderEnabled, Is.EqualTo(expected.BorderEnabled));
        Assert.That(command.ShadowEnabled, Is.EqualTo(expected.BorderShadowEnabled));
        Assert.That(command.BorderBevel, Is.EqualTo(expected.BorderBevel));
        Assert.That(command.BorderOuterWidth, Is.EqualTo(expected.BorderOuterWidth));
        Assert.That(command.BorderInnerWidth, Is.EqualTo(expected.BorderInnerWidth));
        Assert.That(command.BorderOuterSoftness, Is.EqualTo(expected.BorderOuterSoftness));
        Assert.That(command.BorderInnerSoftness, Is.EqualTo(expected.BorderInnerSoftness));
        Assert.That(command.BorderBevelSoftness, Is.EqualTo(expected.BorderBevelSoftness));
        Assert.That(command.BorderBevelPosition, Is.EqualTo(expected.BorderBevelPosition));
        Assert.That(command.BorderOpacity, Is.EqualTo(expected.BorderOpacity));
        Assert.That(command.BorderHue, Is.EqualTo(expected.BorderHue));
        Assert.That(command.BorderSaturation, Is.EqualTo(expected.BorderSaturation));
        Assert.That(command.BorderLuma, Is.EqualTo(expected.BorderLuma));
        Assert.That(command.LightSourceDirection, Is.EqualTo(expected.LightSourceDirection));
        Assert.That(command.LightSourceAltitude, Is.EqualTo(expected.LightSourceAltitude));
        Assert.That(command.MaskEnabled, Is.EqualTo(expected.MaskEnabled));
        Assert.That(command.MaskTop, Is.EqualTo(expected.MaskTop));
        Assert.That(command.MaskBottom, Is.EqualTo(expected.MaskBottom));
        Assert.That(command.MaskLeft, Is.EqualTo(expected.MaskLeft));
        Assert.That(command.MaskRight, Is.EqualTo(expected.MaskRight));
        Assert.That(command.Rate, Is.EqualTo(expected.Rate));
    }
}
