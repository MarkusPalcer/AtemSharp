using AtemSharp.Commands;
using FairlightMixerMasterEqualizerBandUpdateCommand = AtemSharp.Commands.Audio.Fairlight.Master.FairlightMixerMasterEqualizerBandUpdateCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Master;

public class FairlightMixerMasterEqualizerBandUpdateCommandTests : DeserializedCommandTestBase<FairlightMixerMasterEqualizerBandUpdateCommand, FairlightMixerMasterEqualizerBandUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
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


    protected override void CompareCommandProperties(FairlightMixerMasterEqualizerBandUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.BandIndex, Is.EqualTo(expectedData.Band));
        Assert.That(actualCommand.Enabled, Is.EqualTo(expectedData.BandEnabled));
        Assert.That(actualCommand.SupportedShapes, Is.EquivalentTo(DeserializationExtensions.GetComponentsLegacy(expectedData.SupportedShapes)));
        Assert.That(actualCommand.Shape, Is.EqualTo(expectedData.Shape));
        Assert.That(actualCommand.SupportedFrequencyRanges, Is.EquivalentTo(DeserializationExtensions.GetComponentsLegacy(expectedData.SupportedFrequencyRanges)));
        Assert.That(actualCommand.FrequencyRange, Is.EqualTo(expectedData.FrequencyRange));
        Assert.That(actualCommand.Frequency, Is.EqualTo(expectedData.Frequency));
        Assert.That(actualCommand.Gain, Is.EqualTo(expectedData.Gain).Within(0.01));
        Assert.That(actualCommand.QFactor, Is.EqualTo(expectedData.QFactor).Within(0.01));

    }
}
