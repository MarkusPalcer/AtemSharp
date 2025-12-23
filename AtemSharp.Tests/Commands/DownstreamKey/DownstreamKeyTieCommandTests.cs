using AtemSharp.Commands.DownstreamKey;
using AtemSharp.State.Video.DownstreamKeyer;

namespace AtemSharp.Tests.Commands.DownstreamKey;

[TestFixture]
public class DownstreamKeyTieCommandTests : SerializedCommandTestBase<DownstreamKeyTieCommand, DownstreamKeyTieCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public bool Tie { get; set; }
    }

    protected override DownstreamKeyTieCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new DownstreamKeyTieCommand(new DownstreamKeyer
        {
            Id = testCase.Command.Index,
            Properties =
            {
                Tie = testCase.Command.Tie
            }
        });
    }
}
