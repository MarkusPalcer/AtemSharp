using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;
using FairlightMixerSourceCompressorUpdateCommand = AtemSharp.Commands.Audio.Fairlight.Source.FairlightMixerSourceCompressorUpdateCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Source;

internal class FairlightMixerSourceCompressorUpdateCommandTests : DeserializedCommandTestBase<FairlightMixerSourceCompressorUpdateCommand,
    FairlightMixerSourceCompressorUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
        public long SourceId { get; set; }
        public bool CompressorEnabled { get; set; }
        public double Threshold { get; set; }
        public double Ratio { get; set; }
        public double Attack { get; set; }
        public double Hold { get; set; }
        public double Release { get; set; }
    }

    internal override void CompareCommandProperties(FairlightMixerSourceCompressorUpdateCommand actualCommand, CommandData expectedData)
    {
        Assert.That(actualCommand.InputId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.SourceId, Is.EqualTo(expectedData.SourceId));
        Assert.That(actualCommand.CompressorEnabled, Is.EqualTo(expectedData.CompressorEnabled));
        Assert.That(actualCommand.Threshold, Is.EqualTo(expectedData.Threshold).Within(0.01));
        Assert.That(actualCommand.Ratio, Is.EqualTo(expectedData.Ratio).Within(0.01));
        Assert.That(actualCommand.Attack, Is.EqualTo(expectedData.Attack).Within(0.01));
        Assert.That(actualCommand.Hold, Is.EqualTo(expectedData.Hold).Within(0.01));
        Assert.That(actualCommand.Release, Is.EqualTo(expectedData.Release).Within(0.01));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Audio = new FairlightAudioState();
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var actualCommand = state.GetFairlight().Inputs[expectedData.Index].Sources[expectedData.SourceId].Dynamics.Compressor;
        Assert.That(actualCommand.Enabled, Is.EqualTo(expectedData.CompressorEnabled));
        Assert.That(actualCommand.Threshold, Is.EqualTo(expectedData.Threshold).Within(0.01));
        Assert.That(actualCommand.Ratio, Is.EqualTo(expectedData.Ratio).Within(0.01));
        Assert.That(actualCommand.Attack, Is.EqualTo(expectedData.Attack).Within(0.01));
        Assert.That(actualCommand.Hold, Is.EqualTo(expectedData.Hold).Within(0.01));
        Assert.That(actualCommand.Release, Is.EqualTo(expectedData.Release).Within(0.01));
    }
}
