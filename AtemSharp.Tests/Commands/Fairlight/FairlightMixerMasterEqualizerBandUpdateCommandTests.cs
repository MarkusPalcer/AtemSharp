using AtemSharp.Commands.Fairlight;
using AtemSharp.Lib;

namespace AtemSharp.Tests.Commands.Fairlight;

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
        Assert.That(actualCommand.Parameters.BandIndex, Is.EqualTo(expectedData.Band));
        Assert.That(actualCommand.Parameters.Enabled, Is.EqualTo(expectedData.BandEnabled));
        Assert.That(actualCommand.Parameters.SupportedShapes, Is.EquivalentTo(AtemUtil.GetComponents(expectedData.SupportedShapes)));
        Assert.That(actualCommand.Parameters.Shape, Is.EqualTo(expectedData.Shape));
        Assert.That(actualCommand.Parameters.SupportedFrequencyRanges, Is.EquivalentTo(AtemUtil.GetComponents(expectedData.SupportedFrequencyRanges)));
        Assert.That(actualCommand.Parameters.FrequencyRange, Is.EqualTo(expectedData.FrequencyRange));
        Assert.That(actualCommand.Parameters.Frequency, Is.EqualTo(expectedData.Frequency));
        Assert.That(actualCommand.Parameters.Gain, Is.EqualTo(expectedData.Gain).Within(0.01));
        Assert.That(actualCommand.Parameters.QFactor, Is.EqualTo(expectedData.QFactor).Within(0.01));

    }
}
