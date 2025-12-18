using AtemSharp.Commands.DeviceProfile;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DeviceProfile;

[TestFixture]
public class MixEffectBlockConfigCommandTests : DeserializedCommandTestBase<MixEffectBlockConfigCommand,
    MixEffectBlockConfigCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public byte KeyCount { get; set; }
    }

    protected override void CompareCommandProperties(MixEffectBlockConfigCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.Index, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.KeyCount, Is.EqualTo(expectedData.KeyCount));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Info.MixEffects.GetOrCreate(expectedData.Index);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.Info.MixEffects[expectedData.Index].Id, Is.EqualTo(expectedData.Index));
        Assert.That(state.Info.MixEffects[expectedData.Index].KeyCount, Is.EqualTo(expectedData.KeyCount));
    }
}
