using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;
using FairlightMixerSourceUpdateCommand = AtemSharp.Commands.Audio.Fairlight.Source.FairlightMixerSourceUpdateCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Source;

public class FairlightMixerSourceUpdateCommandTests : DeserializedCommandTestBase<FairlightMixerSourceUpdateCommand,
    FairlightMixerSourceUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
        public string SourceId { get; set; } = string.Empty;
        public int FramesDelay { get; set; }
        public double Gain { get; set; }
        public bool HasStereoSimulation { get; set; }
        public double StereoSimulation { get; set; }
        public byte EqualizerBands { get; set; }
        public bool EqualizerEnabled { get; set; }
        public double EqualizerGain { get; set; }
        public double MakeUpGain { get; set; }
        public double Balance { get; set; }
        public double FaderGain { get; set; }
        public FairlightAudioMixOption MixOption { get; set; }
        public FairlightAudioMixOption SupportedMixOptions { get; set; }

        public FairlightAudioSourceType SourceType { get; set; }

        public int MaxFramesDelay { get; set; }
    }

    protected override void CompareCommandProperties(FairlightMixerSourceUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.InputId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.SourceId.ToString(), Is.EqualTo(expectedData.SourceId));
        Assert.That(actualCommand.FramesDelay, Is.EqualTo(expectedData.FramesDelay));
        Assert.That(actualCommand.Gain, Is.EqualTo(expectedData.Gain).Within(0.1));
        Assert.That(actualCommand.HasStereoSimulation, Is.EqualTo(expectedData.HasStereoSimulation));
        Assert.That(actualCommand.StereoSimulation, Is.EqualTo(expectedData.StereoSimulation).Within(0.1));
        Assert.That(actualCommand.BandCount, Is.EqualTo(expectedData.EqualizerBands));
        Assert.That(actualCommand.EqualizerEnabled, Is.EqualTo(expectedData.EqualizerEnabled));
        Assert.That(actualCommand.EqualizerGain, Is.EqualTo(expectedData.EqualizerGain).Within(0.1));
        Assert.That(actualCommand.MakeUpGain, Is.EqualTo(expectedData.MakeUpGain).Within(0.1));
        Assert.That(actualCommand.Balance, Is.EqualTo(expectedData.Balance).Within(0.1));
        Assert.That(actualCommand.FaderGain, Is.EqualTo(expectedData.FaderGain).Within(0.1));
        Assert.That(actualCommand.MixOption, Is.EqualTo(expectedData.MixOption));
        Assert.That(CommandTestUtilities.CombineComponents(actualCommand.SupportedMixOptions),
                    Is.EqualTo(expectedData.SupportedMixOptions));
        Assert.That(actualCommand.SourceType, Is.EqualTo(expectedData.SourceType));
        Assert.That(actualCommand.MaxFramesDelay, Is.EqualTo(expectedData.MaxFramesDelay));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Audio = new FairlightAudioState();
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var actualCommand = state.GetFairlight().Inputs[expectedData.Index].Sources[long.Parse(expectedData.SourceId)];
        Assert.That(actualCommand.InputId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.Id.ToString(), Is.EqualTo(expectedData.SourceId));
        Assert.That(actualCommand.FramesDelay, Is.EqualTo(expectedData.FramesDelay));
        Assert.That(actualCommand.Gain, Is.EqualTo(expectedData.Gain).Within(0.1));
        Assert.That(actualCommand.HasStereoSimulation, Is.EqualTo(expectedData.HasStereoSimulation));
        Assert.That(actualCommand.StereoSimulation, Is.EqualTo(expectedData.StereoSimulation).Within(0.1));
        Assert.That(actualCommand.Equalizer.Bands, Has.Length.EqualTo(expectedData.EqualizerBands));
        Assert.That(actualCommand.Equalizer.Enabled, Is.EqualTo(expectedData.EqualizerEnabled));
        Assert.That(actualCommand.Equalizer.Gain, Is.EqualTo(expectedData.EqualizerGain).Within(0.1));
        Assert.That(actualCommand.Dynamics.MakeUpGain, Is.EqualTo(expectedData.MakeUpGain).Within(0.1));
        Assert.That(actualCommand.Balance, Is.EqualTo(expectedData.Balance).Within(0.1));
        Assert.That(actualCommand.FaderGain, Is.EqualTo(expectedData.FaderGain).Within(0.1));
        Assert.That(actualCommand.MixOption, Is.EqualTo(expectedData.MixOption));
        Assert.That(CommandTestUtilities.CombineComponents(actualCommand.SupportedMixOptions),
                    Is.EqualTo(expectedData.SupportedMixOptions));
        Assert.That(actualCommand.Type, Is.EqualTo(expectedData.SourceType));
        Assert.That(actualCommand.MaxFramesDelay, Is.EqualTo(expectedData.MaxFramesDelay));
    }
}
