using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

[TestFixture]
internal class MixEffectKeyLumaUpdateCommandTests : DeserializedCommandTestBase<MixEffectKeyLumaUpdateCommand,
    MixEffectKeyLumaUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MixEffectIndex { get; set; }
        public byte KeyerIndex { get; set; }
        public bool PreMultiplied { get; set; }
        public double Clip { get; set; }
        public double Gain { get; set; }
        public bool Invert { get; set; }
    }

    internal override void CompareCommandProperties(MixEffectKeyLumaUpdateCommand actualCommand, CommandData expectedData, TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.MixEffectIndex));
        Assert.That(actualCommand.KeyerId, Is.EqualTo(expectedData.KeyerIndex));
        Assert.That(actualCommand.PreMultiplied, Is.EqualTo(expectedData.PreMultiplied));
        Assert.That(actualCommand.Clip, Is.EqualTo(expectedData.Clip).Within(0.10));
        Assert.That(actualCommand.Gain, Is.EqualTo(expectedData.Gain).Within(0.10));
        Assert.That(actualCommand.Invert, Is.EqualTo(expectedData.Invert));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Video.MixEffects.GetOrCreate(expectedData.MixEffectIndex);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var actualCommand = state.Video.MixEffects[expectedData.MixEffectIndex].UpstreamKeyers[expectedData.KeyerIndex].PreMultipliedKey;
        Assert.That(actualCommand.Enabled, Is.EqualTo(expectedData.PreMultiplied));
        Assert.That(actualCommand.Clip, Is.EqualTo(expectedData.Clip).Within(0.10));
        Assert.That(actualCommand.Gain, Is.EqualTo(expectedData.Gain).Within(0.10));
        Assert.That(actualCommand.Inverted, Is.EqualTo(expectedData.Invert));
    }
}
