using AtemSharp.Commands.MixEffects.FadeToBlack;
using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Tests.Commands.MixEffects.FadeToBlack;

public class FadeToBlackRateCommandTests : SerializedCommandTestBase<FadeToBlackRateCommand, FadeToBlackRateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; init; }
        public byte Rate { get; init; }
    }

    protected override FadeToBlackRateCommand CreateSut(TestCaseData testCase)
    {
        return new FadeToBlackRateCommand(new MixEffect
        {
            Id = testCase.Command.Index,
            FadeToBlack =
            {
                Rate = testCase.Command.Rate,
            }
        });
    }
}
