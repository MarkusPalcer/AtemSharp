using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.State.Video.MixEffect.UpstreamKeyer;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

// TODO #83: Capture test data for RunToInfinite case
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
        switch (testCase.Command.KeyFrame)
        {
            case 3:
                return MixEffectKeyRunToCommand.RunToFull(new UpstreamKeyer
                {
                    MixEffectId = testCase.Command.MixEffectIndex,
                    Id = testCase.Command.KeyerIndex
                }, testCase.Command.RunToInfinite);
            case 4:
                return MixEffectKeyRunToCommand.RunToInfinite(new UpstreamKeyer
                {
                    MixEffectId = testCase.Command.MixEffectIndex,
                    Id = testCase.Command.KeyerIndex
                }, testCase.Command.RunToInfinite);
            default:
                return MixEffectKeyRunToCommand.RunToKeyframe(new UpstreamKeyerFlyKeyframe
                {
                    MixEffectId = testCase.Command.MixEffectIndex,
                    UpstreamKeyerId = testCase.Command.KeyerIndex,
                    Id = testCase.Command.KeyFrame
                }, testCase.Command.RunToInfinite);
        }
    }
}
