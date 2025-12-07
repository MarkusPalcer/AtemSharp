using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;
using FairlightMixerSourceDeleteCommand = AtemSharp.Commands.Audio.Fairlight.Source.FairlightMixerSourceDeleteCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Source;

public class FairlightMixerSourceDeleteCommandTests : DeserializedCommandTestBase<FairlightMixerSourceDeleteCommand,
    FairlightMixerSourceDeleteCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
        public string SourceId { get; set; } = string.Empty;
    }

    protected override void CompareCommandProperties(FairlightMixerSourceDeleteCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.InputId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.SourceId.ToString(), Is.EqualTo(expectedData.SourceId));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        // Create an entry to remove
        state.Audio = new FairlightAudioState
        {
            Inputs =
            {
                [expectedData.Index] = new FairlightAudioInput
                {
                    Sources = { [long.Parse(expectedData.SourceId)] = new AtemSharp.State.Audio.Fairlight.Source() }
                }
            }
        };
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.GetFairlight().Inputs[expectedData.Index].Sources, Does.Not.ContainKey(long.Parse(expectedData.SourceId)));
    }

    [Test]
    public void MissingSource()
    {
        var sut = new FairlightMixerSourceDeleteCommand()
        {
            InputId = 1,
            SourceId = 1
        };

        var state = new AtemState
        {
            Audio = new FairlightAudioState
            {
                Inputs =
                {
                    [1] = new FairlightAudioInput()
                }
            }
        };

        sut.ApplyToState(state);

        Assert.That(state.GetFairlight().Inputs.Keys, Is.EquivalentTo(new ushort[] { 1 }));
        Assert.That(state.GetFairlight().Inputs[1].Sources, Is.Empty);
    }

    [Test]
    public void MissingInput()
    {
        var sut = new FairlightMixerSourceDeleteCommand()
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
