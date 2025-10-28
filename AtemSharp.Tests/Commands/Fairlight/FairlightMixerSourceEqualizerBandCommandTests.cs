using AtemSharp.Commands.Fairlight;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Tests.Commands.Fairlight;

public class FairlightMixerSourceEqualizerBandCommandTests : SerializedCommandTestBase<FairlightMixerSourceEqualizerBandCommand,
    FairlightMixerSourceEqualizerBandCommandTests.CommandData>
{
    protected override Range[] GetFloatingPointByteRanges() =>
    [
        (24..28),
        (28..30)
    ];

    public class CommandData : CommandDataBase
    {
        public int Index { get; set; }
        public long SourceId { get; set; }
        public byte Band { get; set; }
        public bool BandEnabled { get; set; }
        public byte Shape { get; set; }
        public byte FrequencyRange { get; set; }
        public uint Frequency { get; set; }
        public double Gain { get; set; }
        public double QFactor { get; set; }
    }

    protected override FairlightMixerSourceEqualizerBandCommand CreateSut(TestCaseData testCase)
    {
        var band = new SourceEqualizerBand
        {
            Id = testCase.Command.Band,
            InputId = (ushort)testCase.Command.Index,
            SourceId = testCase.Command.SourceId,
            Enabled = testCase.Command.BandEnabled,
            Shape = testCase.Command.Shape,
            FrequencyRange = testCase.Command.FrequencyRange,
            Frequency = testCase.Command.Frequency,
            Gain = testCase.Command.Gain,
            QFactor = testCase.Command.QFactor
        };
        return new FairlightMixerSourceEqualizerBandCommand(band);
    }
}
