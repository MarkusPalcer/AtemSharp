using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

public class MixEffectKeyFlyKeyframeStoreCommandTests : SerializedCommandTestBase<MixEffectKeyFlyKeyframeStoreCommand, MixEffectKeyFlyKeyframeStoreCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MixEffectIndex  { get; set; }
        public byte KeyerIndex  { get; set; }
        public byte KeyFrame { get; set; }
    }

    protected override MixEffectKeyFlyKeyframeStoreCommand CreateSut(TestCaseData testCase)
    {
        return new MixEffectKeyFlyKeyframeStoreCommand(new UpstreamKeyerFlyKeyframe
        {
            UpstreamKeyerId = testCase.Command.KeyerIndex,
            MixEffectId = testCase.Command.MixEffectIndex,
            Id = testCase.Command.KeyFrame
        });
    }
}
