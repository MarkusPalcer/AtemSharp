using AtemSharp.Commands.MixEffects.Key;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

public class MixEffectKeyFlyKeyframeUpdateCommandTests : DeserializedCommandTestBase<MixEffectKeyFlyKeyframeUpdateCommand, MixEffectKeyFlyKeyframeUpdateCommandTests.CommandData>
{
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
        public double BorderHue { get; set; }
        public double BorderSaturation { get; set; }
        public double BorderLuma { get; set; }
        public double LightSourceDirection { get; set; }
        public byte LightSourceAltitude { get; set; }
        public double MaskTop { get; set; }
        public double MaskBottom { get; set; }
        public double MaskLeft { get; set; }
        public double MaskRight { get; set; }
    }

    protected override void CompareCommandProperties(MixEffectKeyFlyKeyframeUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.MixEffectIndex));
        Assert.That(actualCommand.UpstreamKeyerId, Is.EqualTo(expectedData.KeyerIndex));
        Assert.That(actualCommand.KeyframeId, Is.EqualTo(expectedData.KeyFrame));
        Assert.That(actualCommand.SizeX, Is.EqualTo(expectedData.SizeX).Within(0.001));
        Assert.That(actualCommand.SizeY, Is.EqualTo(expectedData.SizeY).Within(0.001));
        Assert.That(actualCommand.PositionX, Is.EqualTo(expectedData.PositionX).Within(0.001));
        Assert.That(actualCommand.PositionY, Is.EqualTo(expectedData.PositionY).Within(0.001));
        Assert.That(actualCommand.Rotation, Is.EqualTo(expectedData.Rotation).Within(0.1));
        Assert.That(actualCommand.BorderOuterWidth, Is.EqualTo(expectedData.OuterWidth).Within(0.01));
        Assert.That(actualCommand.BorderInnerWidth, Is.EqualTo(expectedData.InnerWidth).Within(0.01));
        Assert.That(actualCommand.BorderOuterSoftness, Is.EqualTo(expectedData.OuterSoftness));
        Assert.That(actualCommand.BorderInnerSoftness, Is.EqualTo(expectedData.InnerSoftness));
        Assert.That(actualCommand.BorderBevelSoftness, Is.EqualTo(expectedData.BevelSoftness));
        Assert.That(actualCommand.BorderBevelPosition, Is.EqualTo(expectedData.BevelPosition));
        Assert.That(actualCommand.BorderOpacity, Is.EqualTo(expectedData.BorderOpacity));
        Assert.That(actualCommand.BorderHue, Is.EqualTo(expectedData.BorderHue).Within(0.1));
        Assert.That(actualCommand.BorderSaturation, Is.EqualTo(expectedData.BorderSaturation).Within(0.1));
        Assert.That(actualCommand.BorderLuma, Is.EqualTo(expectedData.BorderLuma).Within(0.1));
        Assert.That(actualCommand.LightSourceDirection, Is.EqualTo(expectedData.LightSourceDirection).Within(0.1));
        Assert.That(actualCommand.LightSourceAltitude, Is.EqualTo(expectedData.LightSourceAltitude));
        Assert.That(actualCommand.MaskTop, Is.EqualTo(expectedData.MaskTop).Within(0.001));
        Assert.That(actualCommand.MaskBottom, Is.EqualTo(expectedData.MaskBottom).Within(0.001));
        Assert.That(actualCommand.MaskLeft, Is.EqualTo(expectedData.MaskLeft).Within(0.001));
        Assert.That(actualCommand.MaskRight, Is.EqualTo(expectedData.MaskRight).Within(0.001));
    }
}
