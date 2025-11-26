using AtemSharp.Commands.MixEffects.Key;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

[TestFixture]
public class MixEffectKeyLumaUpdateCommandTests : DeserializedCommandTestBase<MixEffectKeyLumaUpdateCommand,
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

    protected override void CompareCommandProperties(MixEffectKeyLumaUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.MixEffectIndex));
        Assert.That(actualCommand.KeyerId, Is.EqualTo(expectedData.KeyerIndex));
        Assert.That(actualCommand.PreMultiplied, Is.EqualTo(expectedData.PreMultiplied));
        Assert.That(actualCommand.Clip, Is.EqualTo(expectedData.Clip).Within(0.10));
        Assert.That(actualCommand.Gain, Is.EqualTo(expectedData.Gain).Within(0.10));
        Assert.That(actualCommand.Invert, Is.EqualTo(expectedData.Invert));
    }
}
