using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.State.Video.MixEffect.UpstreamKeyer;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

[TestFixture]
public class MixEffectKeyLumaCommandTests : SerializedCommandTestBase<MixEffectKeyLumaCommand,
    MixEffectKeyLumaCommandTests.CommandData>
{
    /// <summary>
    /// Specify which byte ranges contain floating-point values that should be compared with tolerance
    /// </summary>
    protected override Range[] GetFloatingPointByteRanges()
    {
        return [
            4..6, // Clip field (UInt16)
            6..8  // Gain field (UInt16)
        ];
    }

    public class CommandData : CommandDataBase
    {
        public byte MixEffectIndex { get; set; }
        public byte KeyerIndex { get; set; }
        public bool PreMultiplied { get; set; }
        public double Clip { get; set; }
        public double Gain { get; set; }
        public bool Invert { get; set; }
    }

    protected override MixEffectKeyLumaCommand CreateSut(TestCaseData testCase)
    {
        return new MixEffectKeyLumaCommand(new UpstreamKeyer
        {
            Id = testCase.Command.KeyerIndex,
            MixEffectId = testCase.Command.MixEffectIndex,
            PreMultipliedKey =
            {
                Enabled = testCase.Command.PreMultiplied,
                Clip = testCase.Command.Clip,
                Gain = testCase.Command.Gain,
                Inverted = testCase.Command.Invert
            }
        });
    }
}
