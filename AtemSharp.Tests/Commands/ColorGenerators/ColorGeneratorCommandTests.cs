using AtemSharp.Commands.ColorGenerators;
using AtemSharp.State;
using AtemSharp.Types;

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

    protected override ColorGeneratorCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new ColorGeneratorCommand(new ColorGeneratorState
        {
            Id = testCase.Command.Index,
            Color = new HslColor(testCase.Command.Hue, testCase.Command.Saturation, testCase.Command.Luma)
        });
    }
}
