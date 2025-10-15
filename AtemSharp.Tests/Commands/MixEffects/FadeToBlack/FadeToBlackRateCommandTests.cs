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
        var state = new AtemState
        {
            Info =
            {
                Capabilities = new AtemCapabilities()
                {
                    MixEffects = testCase.Command.Index + 1
                }
            }, Video =
            {
                MixEffects =
                {
                    [testCase.Command.Index] = new MixEffect
                    {
                        Index = testCase.Command.Index,
                        FadeToBlack = new FadeToBlackProperties()
                    }
                }
            }
        };

        return new FadeToBlackRateCommand(state, testCase.Command.Index)
        {
            Rate = testCase.Command.Rate
        };
    }
}
