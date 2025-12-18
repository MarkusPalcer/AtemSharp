using AtemSharp.Commands;
using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;
using FairlightMixerSourceEqualizerBandUpdateCommand =
    AtemSharp.Commands.Audio.Fairlight.Source.FairlightMixerSourceEqualizerBandUpdateCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Source;

public class FairlightMixerSourceEqualizerBandUpdateCommandTests : DeserializedCommandTestBase<
    FairlightMixerSourceEqualizerBandUpdateCommand, FairlightMixerSourceEqualizerBandUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
        public long SourceId { get; set; }
        public byte Band { get; set; }
        public bool BandEnabled { get; set; }
        public byte SupportedShapes { get; set; }
        public byte Shape { get; set; }
        public byte SupportedFrequencyRanges { get; set; }
        public byte FrequencyRange { get; set; }
        public uint Frequency { get; set; }
        public double Gain { get; set; }
        public double QFactor { get; set; }
    }

    protected override void CompareCommandProperties(FairlightMixerSourceEqualizerBandUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.InputId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.SourceId, Is.EqualTo(expectedData.SourceId));
        Assert.That(actualCommand.BandIndex, Is.EqualTo(expectedData.Band));
        Assert.That(actualCommand.Enabled, Is.EqualTo(expectedData.BandEnabled));
        Assert.That(actualCommand.SupportedShapes,
                    Is.EquivalentTo(DeserializationExtensions.GetComponentsLegacy(expectedData.SupportedShapes)));
        Assert.That(actualCommand.Shape, Is.EqualTo(expectedData.Shape));
        Assert.That(actualCommand.SupportedFrequencyRanges,
                    Is.EquivalentTo(DeserializationExtensions.GetComponentsLegacy(expectedData.SupportedFrequencyRanges)));
        Assert.That(actualCommand.FrequencyRange, Is.EqualTo(expectedData.FrequencyRange));
        Assert.That(actualCommand.Frequency, Is.EqualTo(expectedData.Frequency));
        Assert.That(actualCommand.Gain, Is.EqualTo(expectedData.Gain).Within(0.01));
        Assert.That(actualCommand.QFactor, Is.EqualTo(expectedData.QFactor).Within(0.01));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        var audio = new FairlightAudioState();
        state.Audio = audio;
        audio.Inputs.GetOrCreate(expectedData.Index).Sources.GetOrCreate(expectedData.SourceId).Equalizer.Bands.GetOrCreate(expectedData.Band);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var actualCommand = state.GetFairlight().Inputs[expectedData.Index].Sources[expectedData.SourceId].Equalizer
                                 .Bands[expectedData.Band];
        Assert.That(actualCommand.Enabled, Is.EqualTo(expectedData.BandEnabled));
        Assert.That(actualCommand.SupportedShapes,
                    Is.EquivalentTo(DeserializationExtensions.GetComponentsLegacy(expectedData.SupportedShapes)));
        Assert.That(actualCommand.Shape, Is.EqualTo(expectedData.Shape));
        Assert.That(actualCommand.SupportedFrequencyRanges,
                    Is.EquivalentTo(DeserializationExtensions.GetComponentsLegacy(expectedData.SupportedFrequencyRanges)));
        Assert.That(actualCommand.FrequencyRange, Is.EqualTo(expectedData.FrequencyRange));
        Assert.That(actualCommand.Frequency, Is.EqualTo(expectedData.Frequency));
        Assert.That(actualCommand.Gain, Is.EqualTo(expectedData.Gain).Within(0.01));
        Assert.That(actualCommand.QFactor, Is.EqualTo(expectedData.QFactor).Within(0.01));
    }
}
