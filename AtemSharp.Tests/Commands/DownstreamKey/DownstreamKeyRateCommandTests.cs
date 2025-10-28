using AtemSharp.Commands.DownstreamKey;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DownstreamKey;

public class DownstreamKeyRateCommandTests : SerializedCommandTestBase<DownstreamKeyRateCommand, DownstreamKeyRateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public byte Rate { get; set; }
    }

    protected override DownstreamKeyRateCommand CreateSut(TestCaseData testCase)
    {
        var state = new DownstreamKeyer
        {
            Id = testCase.Command.Index,
            Properties =
            {
                Rate = testCase.Command.Rate
            }
        };

        return new DownstreamKeyRateCommand(state)
        {
            Rate = testCase.Command.Rate
        };
    }
}
