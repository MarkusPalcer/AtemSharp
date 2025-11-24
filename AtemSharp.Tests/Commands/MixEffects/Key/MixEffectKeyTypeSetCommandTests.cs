using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

public class MixEffectKeyTypeSetCommandTests : SerializedCommandTestBase<MixEffectKeyTypeSetCommand, MixEffectKeyTypeSetCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MixEffectIndex { get; set; }
        public byte KeyerIndex  { get; set; }
        public MixEffectKeyType KeyType { get; set; }
        public bool FlyEnabled  { get; set; }
    }

    protected override MixEffectKeyTypeSetCommand CreateSut(TestCaseData testCase)
    {
        return new MixEffectKeyTypeSetCommand(new UpstreamKeyer
        {
            MixEffectId = testCase.Command.MixEffectIndex,
            Id = testCase.Command.KeyerIndex,
            KeyType = testCase.Command.KeyType,
            FlyEnabled = testCase.Command.FlyEnabled,
        });
    }
}
