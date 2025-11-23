using AtemSharp.Enums.Fairlight;
using FairlightMixerSourceCommand = AtemSharp.Commands.Fairlight.Source.FairlightMixerSourceCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Source;

public class FairlightMixerSourceCommandTests : SerializedCommandTestBase<FairlightMixerSourceCommand, FairlightMixerSourceCommandTests.CommandData>
{
    protected override Range[] GetFloatingPointByteRanges()
        =>
        [
            (20..24), // Gain
            (24..26), // StereoSimulation
            (28..32), // EqualizerGain
            (32..36), // MakeUpGain
            (36..40), // Balance
            (40..44) // FaderGain
        ];


    public class CommandData:CommandDataBase
    {
        public ushort Index { get; set; }
        public string SourceId { get; set; } = string.Empty;
        public byte FramesDelay { get; set; }
        public double Gain { get; set; }
        public double StereoSimulation { get; set; }
        public bool EqualizerEnabled { get; set; }
        public double EqualizerGain { get; set; }
        public double MakeUpGain { get; set; }
        public double Balance { get; set; }
        public double FaderGain { get; set; }
        public FairlightAudioMixOption MixOption { get; set; }
    }

    protected override FairlightMixerSourceCommand CreateSut(TestCaseData testCase)
    {
        var input = new AtemSharp.State.Audio.Fairlight.Source
        {
            InputId = testCase.Command.Index,
            Id = long.Parse(testCase.Command.SourceId),
            FramesDelay = testCase.Command.FramesDelay,
            Gain = testCase.Command.Gain,
            StereoSimulation = testCase.Command.StereoSimulation,
            Equalizer =
            {
                Enabled = testCase.Command.EqualizerEnabled,
                Gain = testCase.Command.EqualizerGain
            },
            Dynamics =
            {
                MakeUpGain = testCase.Command.MakeUpGain
            },
            Balance = testCase.Command.Balance,
            FaderGain = testCase.Command.FaderGain,
            MixOption = testCase.Command.MixOption
        };
        return new FairlightMixerSourceCommand(input);
    }
}
