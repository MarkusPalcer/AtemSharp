using AtemSharp.Commands.DownstreamKey;
using AtemSharp.State.Video.DownstreamKeyer;

namespace AtemSharp.Tests.Commands.DownstreamKey;

[TestFixture]
public class DownstreamKeyAutoCommandV801Tests : SerializedCommandTestBase<DownstreamKeyAutoCommandV801,
    DownstreamKeyAutoCommandV801Tests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public bool IsTowardsOnAir { get; set; }
    }

    protected override DownstreamKeyAutoCommandV801 CreateSut(TestCaseData testCase)
    {
        return new DownstreamKeyAutoCommandV801(new DownstreamKeyer
        {
            Id = testCase.Command.Index,
            IsTowardsOnAir = testCase.Command.IsTowardsOnAir
        });
    }
}
