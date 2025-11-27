using AtemSharp.Commands.DownstreamKey;
using AtemSharp.State.Info;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Commands.DownstreamKey;

public class DownstreamKeyStateUpdateCommandTests : DeserializedCommandTestBase<DownstreamKeyStateUpdateCommand,
    DownstreamKeyStateUpdateCommandTests.CommandData>
{
    [MaxProtocolVersion(ProtocolVersion.V8_0)]
    public class CommandData : CommandDataBase
    {
        public int Index { get; set; }
        public bool OnAir { get; set; }
        public bool InTransition { get; set; }
        public bool IsAuto { get; set; }
        public int RemainingFrames { get; set; }
    }

    protected override void CompareCommandProperties(DownstreamKeyStateUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.Index, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.OnAir, Is.EqualTo(expectedData.OnAir));
        Assert.That(actualCommand.InTransition, Is.EqualTo(expectedData.InTransition));
        Assert.That(actualCommand.IsAuto, Is.EqualTo(expectedData.IsAuto));
        Assert.That(actualCommand.RemainingFrames, Is.EqualTo(expectedData.RemainingFrames));
    }
}
