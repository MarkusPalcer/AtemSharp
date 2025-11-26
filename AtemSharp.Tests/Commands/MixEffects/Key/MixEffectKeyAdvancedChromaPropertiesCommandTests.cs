using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.State.Video.MixEffect.UpstreamKeyer;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

[TestFixture]
public class MixEffectKeyAdvancedChromaPropertiesCommandTests : SerializedCommandTestBase<MixEffectKeyAdvancedChromaPropertiesCommand,
    MixEffectKeyAdvancedChromaPropertiesCommandTests.CommandData>
{
    /// <summary>
    /// Specify which byte ranges contain floating-point values that should be compared with tolerance
    /// </summary>
    protected override Range[] GetFloatingPointByteRanges()
    {
        return
        [
            4..6, // ForegroundLevel field (UInt16)
            6..8, // BackgroundLevel field (UInt16)
            8..10, // KeyEdge field (UInt16)
            10..12, // SpillSuppression field (UInt16)
            12..14, // FlareSuppression field (UInt16)
            14..16, // Brightness field (Int16)
            16..18, // Contrast field (Int16)
            18..20, // Saturation field (UInt16)
            20..22, // Red field (Int16)
            22..24, // Green field (Int16)
            24..26 // Blue field (Int16)
        ];
    }

    public class CommandData : CommandDataBase
    {
        public byte MixEffectIndex { get; set; }
        public byte KeyerIndex { get; set; }
        public double ForegroundLevel { get; set; }
        public double BackgroundLevel { get; set; }
        public double KeyEdge { get; set; }
        public double SpillSuppression { get; set; }
        public double FlareSuppression { get; set; }
        public double Brightness { get; set; }
        public double Contrast { get; set; }
        public double Saturation { get; set; }
        public double Red { get; set; }
        public double Green { get; set; }
        public double Blue { get; set; }
    }

    protected override MixEffectKeyAdvancedChromaPropertiesCommand CreateSut(TestCaseData testCase)
    {
        return new MixEffectKeyAdvancedChromaPropertiesCommand(new UpstreamKeyer
        {
            MixEffectId = testCase.Command.MixEffectIndex,
            Id = testCase.Command.KeyerIndex,
            OnAir = false,
            FillSource = 1000,
            CutSource = 1001,
            AdvancedChromaSettings =
            {
                Properties =
                {
                    ForegroundLevel = testCase.Command.ForegroundLevel,
                    BackgroundLevel = testCase.Command.BackgroundLevel,
                    KeyEdge = testCase.Command.KeyEdge,
                    SpillSuppression = testCase.Command.SpillSuppression,
                    FlareSuppression = testCase.Command.FlareSuppression,
                    Brightness = testCase.Command.Brightness,
                    Contrast = testCase.Command.Contrast,
                    Saturation = testCase.Command.Saturation,
                    Red = testCase.Command.Red,
                    Green = testCase.Command.Green,
                    Blue = testCase.Command.Blue
                }
            }
        });
    }
}
