using AtemSharp.Commands.Fairlight;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Tests.Commands.Fairlight;

public class FairlightMixerMasterEqualizerBandCommandTests : SerializedCommandTestBase<FairlightMixerMasterEqualizerBandCommand, FairlightMixerMasterEqualizerBandCommandTests.CommandData>
{
    protected override Range[] GetFloatingPointByteRanges() =>
    [
        (12..16), // Gain
        (16..19) // QFactor
    ];

    public class CommandData : CommandDataBase
    {
        public byte Band { get; set; }
        public bool BandEnabled { get; set; }
        public byte Shape { get; set; }
        public byte FrequencyRange { get; set; }
        public uint Frequency { get; set; }
        public double Gain { get; set; }
        public double QFactor { get; set; }
    }

    protected override FairlightMixerMasterEqualizerBandCommand CreateSut(TestCaseData testCase)
    {
        var band = new MasterEqualizerBand
        {
            Index = testCase.Command.Band,
            Enabled = testCase.Command.BandEnabled,
            Shape = testCase.Command.Shape,
            FrequencyRange = testCase.Command.FrequencyRange,
            Frequency = testCase.Command.Frequency,
            Gain = testCase.Command.Gain,
            QFactor = testCase.Command.QFactor
        };

        return new FairlightMixerMasterEqualizerBandCommand(band);
    }
}
