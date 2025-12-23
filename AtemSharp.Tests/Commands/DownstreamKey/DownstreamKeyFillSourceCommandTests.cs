using AtemSharp.Commands.DownstreamKey;
using AtemSharp.State.Video.DownstreamKeyer;

namespace AtemSharp.Tests.Commands.DownstreamKey;

[TestFixture]
public class DownstreamKeyFillSourceCommandTests : SerializedCommandTestBase<DownstreamKeyFillSourceCommand,
    DownstreamKeyFillSourceCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public ushort FillSource { get; set; }
    }

    protected override DownstreamKeyFillSourceCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new DownstreamKeyFillSourceCommand(new DownstreamKeyer
        {
            Id = testCase.Command.Index,
            Sources =
            {
                FillSource = testCase.Command.FillSource
            }
        });
    }
}
