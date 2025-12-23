using AtemSharp.Commands.DownstreamKey;
using AtemSharp.State.Video.DownstreamKeyer;

namespace AtemSharp.Tests.Commands.DownstreamKey;

public class DownstreamKeyRateCommandTests : SerializedCommandTestBase<DownstreamKeyRateCommand, DownstreamKeyRateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public byte Rate { get; set; }
    }

    protected override DownstreamKeyRateCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new DownstreamKeyRateCommand(new DownstreamKeyer
        {
            Id = testCase.Command.Index,
            Properties =
            {
                Rate = testCase.Command.Rate
            }
        });
    }
}
