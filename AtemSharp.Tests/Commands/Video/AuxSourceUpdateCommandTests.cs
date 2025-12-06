using AtemSharp.Commands.Video;
using AtemSharp.State;
using AtemSharp.State.Video;

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

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Video.Auxiliaries = AtemStateUtil.CreateArray<AuxiliaryOutput>(expectedData.Id + 1);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.Video.Auxiliaries[expectedData.Id].Id, Is.EqualTo(expectedData.Id));
        Assert.That(state.Video.Auxiliaries[expectedData.Id].Source, Is.EqualTo(expectedData.Source));
    }
}
