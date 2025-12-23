using AtemSharp.Commands.DownstreamKey;
using AtemSharp.State.Info;
using AtemSharp.State.Video.DownstreamKeyer;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Commands.DownstreamKey;

[TestFixture]
public class DownstreamKeyAutoCommandTests : SerializedCommandTestBase<DownstreamKeyAutoCommand,
    DownstreamKeyAutoCommandTests.CommandData>
{
    [MaxProtocolVersion(ProtocolVersion.V8_0)]
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public bool IsTowardsOnAir { get; set; }
    }

    protected override DownstreamKeyAutoCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new DownstreamKeyAutoCommand(new DownstreamKeyer
        {
            Id = testCase.Command.Index,
            IsTowardsOnAir = testCase.Command.IsTowardsOnAir
        });
    }
}
