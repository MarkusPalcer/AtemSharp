using AtemSharp.Commands.ColorGenerators;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.ColorGenerators;

internal class ColorGeneratorUpdateCommandTests : DeserializedCommandTestBase<ColorGeneratorUpdateCommand,
    ColorGeneratorUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public double Hue { get; set; }
        public double Saturation { get; set; }
        public double Luma { get; set; }
    }

    internal override void CompareCommandProperties(ColorGeneratorUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.Id, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.Hue, Is.EqualTo(expectedData.Hue).Within(0.1));
        Assert.That(actualCommand.Saturation, Is.EqualTo(expectedData.Saturation).Within(0.1));
        Assert.That(actualCommand.Luma, Is.EqualTo(expectedData.Luma).Within(0.1));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.ColorGenerators[expectedData.Index].Id, Is.EqualTo(expectedData.Index));
        Assert.That(state.ColorGenerators[expectedData.Index].Color.Hue, Is.EqualTo(expectedData.Hue).Within(0.1));
        Assert.That(state.ColorGenerators[expectedData.Index].Color.Saturation, Is.EqualTo(expectedData.Saturation).Within(0.1));
        Assert.That(state.ColorGenerators[expectedData.Index].Color.Luma, Is.EqualTo(expectedData.Luma).Within(0.1));
    }
}
