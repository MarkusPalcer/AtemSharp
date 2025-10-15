using AtemSharp.Commands.MixEffects.FadeToBlack;

namespace AtemSharp.Tests.Commands.MixEffects.FadeToBlack;

public class FadeToBlackStateCommandTests : DeserializedCommandTestBase<FadeToBlackStateCommand, FadeToBlackStateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public bool IsFullyBlack { get; set; }
        public bool InTransition { get; set; }
        public byte RemainingFrames { get; set; }
    }

    protected override void CompareCommandProperties(FadeToBlackStateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.Index), "Index mismatch");
        Assert.That(actualCommand.IsFullyBlack, Is.EqualTo(expectedData.IsFullyBlack), "IsFullyBlack mismatch");
        Assert.That(actualCommand.InTransition, Is.EqualTo(expectedData.InTransition), "InTransition mismatch");
        Assert.That(actualCommand.RemainingFrames, Is.EqualTo(expectedData.RemainingFrames), "RemainingFrames mismatch");
    }
}
