using AtemSharp.Commands.SuperSource;
using AtemSharp.State;
using AtemSharp.Types.Border;
using AtemSharp.State.Info;
using AtemSharp.State.Video.SuperSource;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Commands.SuperSource;

internal class SuperSourcePropertiesUpdateCommandTests : DeserializedCommandTestBase<SuperSourcePropertiesUpdateCommand,
    SuperSourcePropertiesUpdateCommandTests.CommandData>
{
    [MaxProtocolVersion(ProtocolVersion.V7_5_2)]
    public class CommandData : CommandDataBase
    {
        public ushort ArtFillInput { get; set; }
        public ushort ArtKeyInput { get; set; }
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

    internal override void CompareCommandProperties(SuperSourcePropertiesUpdateCommand actualCommand, CommandData expectedData, TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        Assert.That(actualCommand.ArtFillSource, Is.EqualTo(expectedData.ArtFillInput));
        Assert.That(actualCommand.ArtCutSource, Is.EqualTo(expectedData.ArtKeyInput));
        Assert.That(actualCommand.ArtOption, Is.EqualTo(expectedData.ArtOption));
        Assert.That(actualCommand.ArtPremultiplied, Is.EqualTo(expectedData.ArtPreMultiplied));
        Assert.That(actualCommand.ArtClip, Is.EqualTo(expectedData.ArtClip).Within(0.1));
        Assert.That(actualCommand.ArtGain, Is.EqualTo(expectedData.ArtGain).Within(0.1));
        Assert.That(actualCommand.ArtInvertKey, Is.EqualTo(expectedData.ArtInvertKey));
        Assert.That(actualCommand.BorderEnabled, Is.EqualTo(expectedData.BorderEnabled));
        Assert.That(actualCommand.BorderBevel, Is.EqualTo(expectedData.BorderBevel));
        Assert.That(actualCommand.OuterWidth, Is.EqualTo(expectedData.BorderOuterWidth).Within(0.01));
        Assert.That(actualCommand.InnerWidth, Is.EqualTo(expectedData.BorderInnerWidth).Within(0.01));
        Assert.That(actualCommand.OuterSoftness, Is.EqualTo(expectedData.BorderOuterSoftness));
        Assert.That(actualCommand.InnerSoftness, Is.EqualTo(expectedData.BorderInnerSoftness));
        Assert.That(actualCommand.BevelSoftness, Is.EqualTo(expectedData.BorderBevelSoftness));
        Assert.That(actualCommand.BevelPosition, Is.EqualTo(expectedData.BorderBevelPosition));
        Assert.That(actualCommand.Hue, Is.EqualTo(expectedData.BorderHue).Within(0.1));
        Assert.That(actualCommand.Saturation, Is.EqualTo(expectedData.BorderSaturation).Within(0.1));
        Assert.That(actualCommand.Luma, Is.EqualTo(expectedData.BorderLuma).Within(0.1));
        Assert.That(actualCommand.LightSourceDirection, Is.EqualTo(expectedData.BorderLightSourceDirection).Within(0.1));
        Assert.That(actualCommand.LightSourceAltitude, Is.EqualTo(expectedData.BorderLightSourceAltitude).Within(1));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Video.SuperSources.GetOrCreate(0);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var actualCommand = state.Video.SuperSources[0];
        Assert.That(actualCommand.FillSource, Is.EqualTo(expectedData.ArtFillInput));
        Assert.That(actualCommand.CutSource, Is.EqualTo(expectedData.ArtKeyInput));
        Assert.That(actualCommand.Option, Is.EqualTo(expectedData.ArtOption));
        Assert.That(actualCommand.PreMultipliedKey.Enabled, Is.EqualTo(expectedData.ArtPreMultiplied));
        Assert.That(actualCommand.PreMultipliedKey.Clip, Is.EqualTo(expectedData.ArtClip).Within(0.1));
        Assert.That(actualCommand.PreMultipliedKey.Gain, Is.EqualTo(expectedData.ArtGain).Within(0.1));
        Assert.That(actualCommand.PreMultipliedKey.Inverted, Is.EqualTo(expectedData.ArtInvertKey));
        Assert.That(actualCommand.Border.Enabled, Is.EqualTo(expectedData.BorderEnabled));
        Assert.That(actualCommand.Border.Bevel, Is.EqualTo(expectedData.BorderBevel));
        Assert.That(actualCommand.Border.OuterWidth, Is.EqualTo(expectedData.BorderOuterWidth).Within(0.01));
        Assert.That(actualCommand.Border.InnerWidth, Is.EqualTo(expectedData.BorderInnerWidth).Within(0.01));
        Assert.That(actualCommand.Border.OuterSoftness, Is.EqualTo(expectedData.BorderOuterSoftness));
        Assert.That(actualCommand.Border.InnerSoftness, Is.EqualTo(expectedData.BorderInnerSoftness));
        Assert.That(actualCommand.Border.BevelSoftness, Is.EqualTo(expectedData.BorderBevelSoftness));
        Assert.That(actualCommand.Border.BevelPosition, Is.EqualTo(expectedData.BorderBevelPosition));
        Assert.That(actualCommand.Border.Color.Hue, Is.EqualTo(expectedData.BorderHue).Within(0.1));
        Assert.That(actualCommand.Border.Color.Saturation, Is.EqualTo(expectedData.BorderSaturation).Within(0.1));
        Assert.That(actualCommand.Border.Color.Luma, Is.EqualTo(expectedData.BorderLuma).Within(0.1));
        Assert.That(actualCommand.Shadow.LightSourceDirection, Is.EqualTo(expectedData.BorderLightSourceDirection).Within(0.1));
        Assert.That(actualCommand.Shadow.LightSourceAltitude, Is.EqualTo(expectedData.BorderLightSourceAltitude).Within(1));
    }
}
