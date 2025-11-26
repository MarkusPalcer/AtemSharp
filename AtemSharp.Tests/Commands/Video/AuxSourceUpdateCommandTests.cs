using AtemSharp.Commands.Video;

namespace AtemSharp.Tests.Commands.Video;

[TestFixture]
public class AuxSourceUpdateCommandTests : DeserializedCommandTestBase<AuxSourceUpdateCommand,
    AuxSourceUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Id { get; set; }
        public int Source { get; set; }
    }

    protected override void CompareCommandProperties(AuxSourceUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.AuxId, Is.EqualTo(expectedData.Id));
        Assert.That(actualCommand.Source, Is.EqualTo(expectedData.Source));
    }
}
