using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.State.Video.MixEffect.UpstreamKeyer;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

public class MixEffectKeyCutSourceSetCommandTests : SerializedCommandTestBase<MixEffectKeyCutSourceSetCommand, MixEffectKeyCutSourceSetCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MixEffectIndex { get; set; }
        public byte KeyerIndex { get; set; }
        public ushort CutSource { get; set; }
    }


    protected override MixEffectKeyCutSourceSetCommand CreateSut(TestCaseData testCase)
    {
        return new MixEffectKeyCutSourceSetCommand(new UpstreamKeyer
        {
            Id = testCase.Command.KeyerIndex,
            CutSource = testCase.Command.CutSource,
            MixEffectId = testCase.Command.MixEffectIndex,
        });
    }
}
