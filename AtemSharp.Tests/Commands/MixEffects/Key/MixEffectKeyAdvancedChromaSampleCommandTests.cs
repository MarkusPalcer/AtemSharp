using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.State.Video.MixEffect.UpstreamKeyer;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

[TestFixture]
public class MixEffectKeyAdvancedChromaSampleCommandTests : SerializedCommandTestBase<MixEffectKeyAdvancedChromaSampleCommand,
    MixEffectKeyAdvancedChromaSampleCommandTests.CommandData>
{
    /// <summary>
    /// Specify which byte ranges contain floating-point values that should be compared with tolerance
    /// </summary>
    protected override Range[] GetFloatingPointByteRanges()
    {
        return [
            6..8,   // CursorX field (Int16)
            8..10,  // CursorY field (Int16)
            10..12, // CursorSize field (UInt16)
            12..14, // SampledY field (UInt16)
            14..16, // SampledCb field (Int16)
            16..18  // SampledCr field (Int16)
        ];
    }

    public class CommandData : CommandDataBase
    {
        public byte MixEffectIndex { get; set; }
        public byte KeyerIndex { get; set; }
        public bool EnableCursor { get; set; }
        public bool Preview { get; set; }
        public double CursorX { get; set; }
        public double CursorY { get; set; }
        public double CursorSize { get; set; }
        public double SampledY { get; set; }
        public double SampledCb { get; set; }
        public double SampledCr { get; set; }
    }

    protected override MixEffectKeyAdvancedChromaSampleCommand CreateSut(TestCaseData testCase)
    {
        // Create state with the required mix effect and upstream keyer
        var state = new UpstreamKeyer
        {
            Id = testCase.Command.KeyerIndex,
            MixEffectId = testCase.Command.MixEffectIndex,
            AdvancedChromaSettings =
            {
                Sample =
                {
                    EnableCursor = testCase.Command.EnableCursor,
                    Preview = testCase.Command.Preview,
                    CursorX = testCase.Command.CursorX,
                    CursorY = testCase.Command.CursorY,
                    CursorSize = testCase.Command.CursorSize,
                    SampledY = testCase.Command.SampledY,
                    SampledCb = testCase.Command.SampledCb,
                    SampledCr = testCase.Command.SampledCr
                }
            }
        };

        return new MixEffectKeyAdvancedChromaSampleCommand(state);
    }
}
