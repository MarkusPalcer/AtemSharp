using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

public class MixEffectKeyFillSourceSetCommandTests : SerializedCommandTestBase<MixEffectKeyFillSourceSetCommand, MixEffectKeyFillSourceSetCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MixEffectIndex { get; set; }
        public byte KeyerIndex { get; set; }
        public ushort FillSource { get; set; }
    }

    protected override MixEffectKeyFillSourceSetCommand CreateSut(TestCaseData testCase)
    {
        return new MixEffectKeyFillSourceSetCommand(new UpstreamKeyer
        {
            Id = testCase.Command.KeyerIndex,
            FillSource = testCase.Command.FillSource,
            MixEffectId = testCase.Command.MixEffectIndex,
        });
    }
}
