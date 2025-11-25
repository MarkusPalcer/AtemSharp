using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.State.Video.MixEffect.UpstreamKeyer;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

public class MixEffectKeyAdvancedChromaSampleResetCommandTests : SerializedCommandTestBase<MixEffectKeyAdvancedChromaSampleResetCommand, MixEffectKeyAdvancedChromaSampleResetCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MixEffectIndex  { get; set; }
        public byte KeyerIndex { get; set; }
        public bool ChromaCorrection { get; set; }
        public bool ColorAdjustments { get; set; }
        public bool KeyAdjustments { get; set; }
    }

    protected override MixEffectKeyAdvancedChromaSampleResetCommand CreateSut(TestCaseData testCase)
    {
        return new MixEffectKeyAdvancedChromaSampleResetCommand(new UpstreamKeyer
        {
            Id = testCase.Command.KeyerIndex,
            MixEffectId = testCase.Command.MixEffectIndex,
        })
        {
            ResetKeyAdjustments = testCase.Command.KeyAdjustments,
            ResetChromaCorrection = testCase.Command.ChromaCorrection,
            ResetColorAdjustments =  testCase.Command.ColorAdjustments
        };
    }
}
