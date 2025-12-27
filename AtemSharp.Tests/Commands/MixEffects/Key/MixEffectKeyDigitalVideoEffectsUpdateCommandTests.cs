using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.State;
using AtemSharp.Types.Border;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

internal class MixEffectKeyDigitalVideoEffectsUpdateCommandTests : DeserializedCommandTestBase<MixEffectKeyDigitalVideoEffectsUpdateCommand,
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

    internal override void CompareCommandProperties(MixEffectKeyDigitalVideoEffectsUpdateCommand command, CommandData expected)
    {
        Assert.That(command.MixEffectId, Is.EqualTo(expected.MixEffectIndex));
        Assert.That(command.KeyerId, Is.EqualTo(expected.KeyerIndex));
        Assert.That(command.SizeX, Is.EqualTo(expected.SizeX).Within(0.001));
        Assert.That(command.SizeY, Is.EqualTo(expected.SizeY).Within(0.001));
        Assert.That(command.PositionX, Is.EqualTo(expected.PositionX).Within(0.001));
        Assert.That(command.PositionY, Is.EqualTo(expected.PositionY).Within(0.001));
        Assert.That(command.Rotation, Is.EqualTo(expected.Rotation).Within(0.1));
        Assert.That(command.BorderEnabled, Is.EqualTo(expected.BorderEnabled));
        Assert.That(command.ShadowEnabled, Is.EqualTo(expected.BorderShadowEnabled));
        Assert.That(command.BorderBevel, Is.EqualTo(expected.BorderBevel));
        Assert.That(command.BorderOuterWidth, Is.EqualTo(expected.BorderOuterWidth).Within(0.01));
        Assert.That(command.BorderInnerWidth, Is.EqualTo(expected.BorderInnerWidth).Within(0.01));
        Assert.That(command.BorderOuterSoftness, Is.EqualTo(expected.BorderOuterSoftness));
        Assert.That(command.BorderInnerSoftness, Is.EqualTo(expected.BorderInnerSoftness));
        Assert.That(command.BorderBevelSoftness, Is.EqualTo(expected.BorderBevelSoftness));
        Assert.That(command.BorderBevelPosition, Is.EqualTo(expected.BorderBevelPosition));
        Assert.That(command.BorderOpacity, Is.EqualTo(expected.BorderOpacity));
        Assert.That(command.BorderHue, Is.EqualTo(expected.BorderHue).Within(0.1));
        Assert.That(command.BorderSaturation, Is.EqualTo(expected.BorderSaturation).Within(0.1));
        Assert.That(command.BorderLuma, Is.EqualTo(expected.BorderLuma).Within(0.1));
        Assert.That(command.LightSourceDirection, Is.EqualTo(expected.LightSourceDirection).Within(0.1));
        Assert.That(command.LightSourceAltitude, Is.EqualTo(expected.LightSourceAltitude));
        Assert.That(command.MaskEnabled, Is.EqualTo(expected.MaskEnabled));
        Assert.That(command.MaskTop, Is.EqualTo(expected.MaskTop).Within(0.001));
        Assert.That(command.MaskBottom, Is.EqualTo(expected.MaskBottom).Within(0.001));
        Assert.That(command.MaskLeft, Is.EqualTo(expected.MaskLeft).Within(0.001));
        Assert.That(command.MaskRight, Is.EqualTo(expected.MaskRight).Within(0.001));
        Assert.That(command.Rate, Is.EqualTo(expected.Rate));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Video.MixEffects.GetOrCreate(expectedData.MixEffectIndex);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expected)
    {
        var dve = state.Video.MixEffects[expected.MixEffectIndex].UpstreamKeyers[expected.KeyerIndex]
                           .DigitalVideoEffectsSettings;
        Assert.That(dve.Size.Width, Is.EqualTo(expected.SizeX).Within(0.001));
        Assert.That(dve.Size.Height, Is.EqualTo(expected.SizeY).Within(0.001));
        Assert.That(dve.Location.X, Is.EqualTo(expected.PositionX).Within(0.001));
        Assert.That(dve.Location.Y, Is.EqualTo(expected.PositionY).Within(0.001));
        Assert.That(dve.Rotation, Is.EqualTo(expected.Rotation).Within(0.1));
        Assert.That(dve.Border.Enabled, Is.EqualTo(expected.BorderEnabled));
        Assert.That(dve.Border.Bevel, Is.EqualTo(expected.BorderBevel));
        Assert.That(dve.Border.OuterWidth, Is.EqualTo(expected.BorderOuterWidth).Within(0.01));
        Assert.That(dve.Border.InnerWidth, Is.EqualTo(expected.BorderInnerWidth).Within(0.01));
        Assert.That(dve.Border.OuterSoftness, Is.EqualTo(expected.BorderOuterSoftness));
        Assert.That(dve.Border.InnerSoftness, Is.EqualTo(expected.BorderInnerSoftness));
        Assert.That(dve.Border.BevelSoftness, Is.EqualTo(expected.BorderBevelSoftness));
        Assert.That(dve.Border.BevelPosition, Is.EqualTo(expected.BorderBevelPosition));
        Assert.That(dve.Border.Opacity, Is.EqualTo(expected.BorderOpacity));
        Assert.That(dve.Border.Color.Hue, Is.EqualTo(expected.BorderHue).Within(0.1));
        Assert.That(dve.Border.Color.Saturation, Is.EqualTo(expected.BorderSaturation).Within(0.1));
        Assert.That(dve.Border.Color.Luma, Is.EqualTo(expected.BorderLuma).Within(0.1));
        Assert.That(dve.Shadow.LightSourceDirection, Is.EqualTo(expected.LightSourceDirection).Within(0.1));
        Assert.That(dve.Shadow.LightSourceAltitude, Is.EqualTo(expected.LightSourceAltitude));
        Assert.That(dve.Shadow.Enabled, Is.EqualTo(expected.BorderShadowEnabled));
        Assert.That(dve.Mask.Enabled, Is.EqualTo(expected.MaskEnabled));
        Assert.That(dve.Mask.Top, Is.EqualTo(expected.MaskTop).Within(0.001));
        Assert.That(dve.Mask.Bottom, Is.EqualTo(expected.MaskBottom).Within(0.001));
        Assert.That(dve.Mask.Left, Is.EqualTo(expected.MaskLeft).Within(0.001));
        Assert.That(dve.Mask.Right, Is.EqualTo(expected.MaskRight).Within(0.001));
        Assert.That(dve.Rate, Is.EqualTo(expected.Rate));
    }
}
