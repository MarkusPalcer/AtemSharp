using AtemSharp.Commands.DownstreamKey;
using AtemSharp.State.Video.DownstreamKeyer;

namespace AtemSharp.Tests.Commands.DownstreamKey;

[TestFixture]
public class DownstreamKeyOnAirCommandTests : SerializedCommandTestBase<DownstreamKeyOnAirCommand,
    DownstreamKeyOnAirCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public bool OnAir { get; set; }
    }

    protected override DownstreamKeyOnAirCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new DownstreamKeyOnAirCommand(new DownstreamKeyer
        {
            Id = testCase.Command.Index,
            OnAir = testCase.Command.OnAir,
        });
    }
}
