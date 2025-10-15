using AtemSharp.Commands.DownstreamKey;

namespace AtemSharp.Tests.Commands.DownstreamKey;

public class DownstreamKeyStateV8CommandTests : DeserializedCommandTestBase<DownstreamKeyStateV8Command, DownstreamKeyStateV8CommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Index { get; set; }
        public bool OnAir { get; set; }
        public bool InTransition { get; set; }
        public bool IsAuto { get; set; }
        public int RemainingFrames { get; set; }

        public bool IsTowardsOnAir { get; set; }
    }

    protected override void CompareCommandProperties(DownstreamKeyStateV8Command actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.Multiple(() =>
        {
            Assert.That(actualCommand.Index, Is.EqualTo(expectedData.Index), $"{testCase.Name} - Index");
            Assert.That(actualCommand.OnAir, Is.EqualTo(expectedData.OnAir), $"{testCase.Name} - OnAir");
            Assert.That(actualCommand.InTransition, Is.EqualTo(expectedData.InTransition), $"{testCase.Name} - InTransition");
            Assert.That(actualCommand.IsAuto, Is.EqualTo(expectedData.IsAuto), $"{testCase.Name} - IsAuto");
            Assert.That(actualCommand.RemainingFrames, Is.EqualTo(expectedData.RemainingFrames), $"{testCase.Name} - RemainingFrames");
            Assert.That(actualCommand.IsTowardsOnAir, Is.EqualTo(expectedData.IsTowardsOnAir), $"{testCase.Name} - IsTowardsOnAir");
        });
    }
}
