using AtemSharp.Commands.MixEffects.FadeToBlack;

namespace AtemSharp.Tests.Commands.MixEffects.FadeToBlack;

public class FadeToBlackRateUpdateCommandTests : DeserializedCommandTestBase<FadeToBlackRateUpdateCommand, FadeToBlackRateUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; init; }
        public ushort Rate { get; init; }
    }

    protected override void CompareCommandProperties(FadeToBlackRateUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.Multiple(() =>
        {
            Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.Index), "MixEffectId");
            Assert.That(actualCommand.Rate, Is.EqualTo(expectedData.Rate), "Rate");
        });
    }
}
