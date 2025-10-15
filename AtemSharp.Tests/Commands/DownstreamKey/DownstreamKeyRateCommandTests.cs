using AtemSharp.Commands.DownstreamKey;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DownstreamKey;

public class DownstreamKeyRateCommandTests : SerializedCommandTestBase<DownstreamKeyRateCommand, DownstreamKeyRateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Index { get; set; }
        public byte Rate { get; set; }
    }

    protected override DownstreamKeyRateCommand CreateSut(TestCaseData testCase)
    {
        var state = new AtemState
        {
            Video =
            {
                DownstreamKeyers = AtemStateUtil.CreateArray<DownstreamKeyer>(testCase.Command.Index + 1)
            }
        };

        state.Video.DownstreamKeyers[testCase.Command.Index].Properties = new();

        return new DownstreamKeyRateCommand(state, (byte)testCase.Command.Index)
        {
            Rate = testCase.Command.Rate
        };
    }
}
