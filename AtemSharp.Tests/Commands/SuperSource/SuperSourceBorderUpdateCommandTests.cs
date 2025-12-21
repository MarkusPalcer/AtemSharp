using AtemSharp.Commands.SuperSource;
using AtemSharp.State;
using AtemSharp.Types.Border;

namespace AtemSharp.Tests.Commands.SuperSource;

public class SuperSourceBorderUpdateCommandTests : DeserializedCommandTestBase<SuperSourceBorderUpdateCommand,
    SuperSourceBorderUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte SSrcId { get; set; }
        public bool Enabled { get; set; }
        public BorderBevel Bevel { get; set; }
        public double OuterWidth { get; set; }
        public double InnerWidth { get; set; }
        public byte OuterSoftness { get; set; }
        public byte InnerSoftness { get; set; }
        public byte BevelSoftness { get; set; }
        public double BevelPosition { get; set; }
        public double Hue { get; set; }
        public double Saturation { get; set; }
        public double Luma { get; set; }
        public double LightSourceDirection { get; set; }
        public double LightSourceAltitude { get; set; }
    }

    protected override void CompareCommandProperties(SuperSourceBorderUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.SuperSourceId, Is.EqualTo(expectedData.SSrcId));
        Assert.That(actualCommand.BorderEnabled, Is.EqualTo(expectedData.Enabled));
        Assert.That(actualCommand.BorderBevel, Is.EqualTo(expectedData.Bevel));
        Assert.That(actualCommand.OuterWidth, Is.EqualTo(expectedData.OuterWidth).Within(0.01));
        Assert.That(actualCommand.InnerWidth, Is.EqualTo(expectedData.InnerWidth).Within(0.01));
        Assert.That(actualCommand.OuterSoftness, Is.EqualTo(expectedData.OuterSoftness));
        Assert.That(actualCommand.InnerSoftness, Is.EqualTo(expectedData.InnerSoftness));
        Assert.That(actualCommand.BevelSoftness, Is.EqualTo(expectedData.BevelSoftness));
        Assert.That(actualCommand.BevelPosition, Is.EqualTo(expectedData.BevelPosition));
        Assert.That(actualCommand.Hue, Is.EqualTo(expectedData.Hue).Within(0.1));
        Assert.That(actualCommand.Saturation, Is.EqualTo(expectedData.Saturation).Within(0.1));
        Assert.That(actualCommand.Luma, Is.EqualTo(expectedData.Luma).Within(0.1));
        Assert.That(actualCommand.LightSourceDirection, Is.EqualTo(expectedData.LightSourceDirection).Within(0.1));
        Assert.That(actualCommand.LightSourceAltitude, Is.EqualTo(expectedData.LightSourceAltitude).Within(1));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Video.SuperSources.GetOrCreate(expectedData.SSrcId);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var actualCommand = state.Video.SuperSources[expectedData.SSrcId];
        Assert.That(actualCommand.Border.Enabled, Is.EqualTo(expectedData.Enabled));
        Assert.That(actualCommand.Border.Bevel, Is.EqualTo(expectedData.Bevel));
        Assert.That(actualCommand.Border.OuterWidth, Is.EqualTo(expectedData.OuterWidth).Within(0.01));
        Assert.That(actualCommand.Border.InnerWidth, Is.EqualTo(expectedData.InnerWidth).Within(0.01));
        Assert.That(actualCommand.Border.OuterSoftness, Is.EqualTo(expectedData.OuterSoftness));
        Assert.That(actualCommand.Border.InnerSoftness, Is.EqualTo(expectedData.InnerSoftness));
        Assert.That(actualCommand.Border.BevelSoftness, Is.EqualTo(expectedData.BevelSoftness));
        Assert.That(actualCommand.Border.BevelPosition, Is.EqualTo(expectedData.BevelPosition));
        Assert.That(actualCommand.Border.Color.Hue, Is.EqualTo(expectedData.Hue).Within(0.1));
        Assert.That(actualCommand.Border.Color.Saturation, Is.EqualTo(expectedData.Saturation).Within(0.1));
        Assert.That(actualCommand.Border.Color.Luma, Is.EqualTo(expectedData.Luma).Within(0.1));
        Assert.That(actualCommand.Shadow.LightSourceDirection, Is.EqualTo(expectedData.LightSourceDirection).Within(0.1));
        Assert.That(actualCommand.Shadow.LightSourceAltitude, Is.EqualTo(expectedData.LightSourceAltitude).Within(1));
    }
}
