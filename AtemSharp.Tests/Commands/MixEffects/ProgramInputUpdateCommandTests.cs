using AtemSharp.Commands.MixEffects;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects;

[TestFixture]
internal class ProgramInputUpdateCommandTests : DeserializedCommandTestBase<ProgramInputUpdateCommand,
    ProgramInputUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public int Source { get; set; }
    }

    internal override void CompareCommandProperties(ProgramInputUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.Source, Is.EqualTo(expectedData.Source));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Video.MixEffects.GetOrCreate(expectedData.Index);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.Video.MixEffects[expectedData.Index].ProgramInput, Is.EqualTo(expectedData.Source));
    }
}
