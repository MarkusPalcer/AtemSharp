using AtemSharp.Commands;

namespace AtemSharp.Tests.Commands;

public class ColorGeneratorUpdateCommandTests : DeserializedCommandTestBase<ColorGeneratorUpdateCommand, ColorGeneratorUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Index { get; set; }
        public double Hue { get; set; }
        public double Saturation { get; set; }
        public double Luma { get; set; }
    }

    protected override void CompareCommandProperties(ColorGeneratorUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.Id, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.Hue, Is.EqualTo(expectedData.Hue).Within(0.1));
        Assert.That(actualCommand.Saturation, Is.EqualTo(expectedData.Saturation).Within(0.1));
        Assert.That(actualCommand.Luma, Is.EqualTo(expectedData.Luma).Within(0.1));
    }
}
