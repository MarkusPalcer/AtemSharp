using AtemSharp.Commands.Video;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Video;

[TestFixture]
internal class AuxSourceUpdateCommandTests : DeserializedCommandTestBase<AuxSourceUpdateCommand,
    AuxSourceUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Id { get; set; }
        public int Source { get; set; }
    }

    internal override void CompareCommandProperties(AuxSourceUpdateCommand actualCommand, CommandData expectedData)
    {
        Assert.That(actualCommand.AuxId, Is.EqualTo(expectedData.Id));
        Assert.That(actualCommand.Source, Is.EqualTo(expectedData.Source));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Video.Auxiliaries.GetOrCreate(expectedData.Id);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.Video.Auxiliaries[expectedData.Id].Id, Is.EqualTo(expectedData.Id));
        Assert.That(state.Video.Auxiliaries[expectedData.Id].Source, Is.EqualTo(expectedData.Source));
    }
}
