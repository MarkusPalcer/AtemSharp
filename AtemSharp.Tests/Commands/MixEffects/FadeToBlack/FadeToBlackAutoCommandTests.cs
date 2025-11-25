using AtemSharp.Commands.MixEffects.FadeToBlack;
using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Tests.Commands.MixEffects.FadeToBlack;

public class FadeToBlackAutoCommandTests : SerializedCommandTestBase<FadeToBlackAutoCommand, FadeToBlackAutoCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
    }

    protected override FadeToBlackAutoCommand CreateSut(TestCaseData testCase)
    {
        return new FadeToBlackAutoCommand(new MixEffect { Id = testCase.Command.Index });
    }
}
