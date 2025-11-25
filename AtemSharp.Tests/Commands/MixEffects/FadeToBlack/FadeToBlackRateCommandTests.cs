using AtemSharp.Commands.MixEffects.FadeToBlack;
using AtemSharp.State;

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
        var mixEffect = new MixEffect
        {
            Id = testCase.Command.Index,
            FadeToBlack = new FadeToBlackProperties()
        };

        return new FadeToBlackRateCommand(mixEffect)
        {
            Rate = testCase.Command.Rate
        };
    }
}
