using AtemSharp.State.Audio.Fairlight;
using FairlightMixerMasterEqualizerBandCommand = AtemSharp.Commands.Audio.Fairlight.Master.FairlightMixerMasterEqualizerBandCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Master;

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
        public Shape Shape { get; set; }
        public FrequencyRange FrequencyRange { get; set; }
        public uint Frequency { get; set; }
        public double Gain { get; set; }
        public double QFactor { get; set; }
    }

    protected override FairlightMixerMasterEqualizerBandCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new FairlightMixerMasterEqualizerBandCommand(new MasterEqualizerBand
        {
            Id = testCase.Command.Band,
            Enabled = testCase.Command.BandEnabled,
            Shape = testCase.Command.Shape,
            FrequencyRange = testCase.Command.FrequencyRange,
            Frequency = testCase.Command.Frequency,
            Gain = testCase.Command.Gain,
            QFactor = testCase.Command.QFactor
        });
    }
}
