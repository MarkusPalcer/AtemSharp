using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;
using FairlightMixerSourceDeleteCommand = AtemSharp.Commands.Audio.Fairlight.Source.FairlightMixerSourceDeleteCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Source;

internal class FairlightMixerSourceDeleteCommandTests : DeserializedCommandTestBase<FairlightMixerSourceDeleteCommand,
    FairlightMixerSourceDeleteCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
        public string SourceId { get; set; } = string.Empty;
    }

    internal override void CompareCommandProperties(FairlightMixerSourceDeleteCommand actualCommand, CommandData expectedData, TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        Assert.That(actualCommand.InputId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.SourceId.ToString(), Is.EqualTo(expectedData.SourceId));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        // Create an entry to remove
        var audio = new FairlightAudioState();
        audio.Inputs.GetOrCreate(expectedData.Index).Sources.GetOrCreate(long.Parse(expectedData.SourceId));
        state.Audio = audio;
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.GetFairlight().Inputs[expectedData.Index].Sources.Select(x => x.Id), Does.Not.Contain(long.Parse(expectedData.SourceId)));
    }

    [Test]
    public void MissingSource()
    {
        var sut = new FairlightMixerSourceDeleteCommand
        {
            InputId = 1,
            SourceId = 1
        };

        var audio = new FairlightAudioState();
        audio.Inputs.GetOrCreate(1);

        var state = new AtemState
        {
            Audio = audio
        };

        sut.ApplyToState(state);

        Assert.That(state.GetFairlight().Inputs.Select(x => x.Id), Is.EquivalentTo(new ushort[] { 1 }));
        Assert.That(state.GetFairlight().Inputs[1].Sources, Is.Empty);
    }

    [Test]
    public void MissingInput()
    {
        var sut = new FairlightMixerSourceDeleteCommand
        {
            InputId = 1,
            SourceId = 1
        };

        var state = new AtemState
        {
            Audio = new FairlightAudioState()
        };

        sut.ApplyToState(state);

        Assert.That(state.GetFairlight().Inputs, Is.Empty);
    }
}
