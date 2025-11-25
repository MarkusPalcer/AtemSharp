using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.Enums;
using AtemSharp.State.Video.MixEffect.UpstreamKeyer;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

public class MixEffectKeyRunToCommandTests : SerializedCommandTestBase<MixEffectKeyRunToCommand, MixEffectKeyRunToCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MixEffectIndex { get; set; }
        public byte KeyerIndex  { get; set; }
        public byte KeyFrame { get; set; }
        public FlyKeyDirection RunToInfinite { get; set; }
    }

    protected override MixEffectKeyRunToCommand CreateSut(TestCaseData testCase)
    {
        return new MixEffectKeyRunToCommand(new UpstreamKeyerFlyKeyframe
        {
            MixEffectId = testCase.Command.MixEffectIndex,
            UpstreamKeyerId = testCase.Command.KeyerIndex,
            Id = testCase.Command.KeyFrame
        })
        {
            Direction = testCase.Command.RunToInfinite
        };
    }
}
