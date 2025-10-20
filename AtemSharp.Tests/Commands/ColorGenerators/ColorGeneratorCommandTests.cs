using AtemSharp.Commands.ColorGenerators;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.ColorGenerators;

public class ColorGeneratorCommandTests : SerializedCommandTestBase<ColorGeneratorCommand, ColorGeneratorCommandTests.CommandData>
{
    protected override Range[] GetFloatingPointByteRanges() =>
    [
        2..4, // Hue
        4..6, // Saturation
        6..8 // Luma
    ];

    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public double Hue { get; set; }
        public double Saturation { get; set; }
        public double Luma { get; set; }
    }

    protected override ColorGeneratorCommand CreateSut(TestCaseData testCase)
    {
        var state = new AtemState
        {
            ColorGenerators =
            {
                [testCase.Command.Index] = new()
            }
        };

        return new ColorGeneratorCommand(state, testCase.Command.Index)
        {
            Hue = testCase.Command.Hue,
            Saturation = testCase.Command.Saturation,
            Luma = testCase.Command.Luma
        };
    }
}
