using AtemSharp.Commands.Fairlight;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Tests.Commands.Fairlight;

public class FairlightMixerMasterCommandTests : SerializedCommandTestBase<FairlightMixerMasterCommand, FairlightMixerMasterCommandTests.CommandData>
{
    protected override Range[] GetFloatingPointByteRanges() =>
    [
        new(4, 8),   // EqualizerGain
        new(8, 12),  // MakeUpGain
        new(12, 16)  // Gain
    ];

    public class CommandData : CommandDataBase
    {
        public bool EqualizerEnabled { get; set; }
        public double EqualizerGain { get; set; }
        public double MakeUpGain { get; set; }
        public bool FollowFadeToBlack { get; set; }
        public double Gain { get; set; }
    }

    protected override FairlightMixerMasterCommand CreateSut(TestCaseData testCase)
    {
        var master = new MasterProperties
        {
            Equalizer =
            {
                Enabled = testCase.Command.EqualizerEnabled,
                Gain = testCase.Command.EqualizerGain
            },
            Dynamics =
            {
                MakeUpGain = testCase.Command.MakeUpGain
            },
            FollowFadeToBlack = testCase.Command.FollowFadeToBlack,
            FaderGain = testCase.Command.Gain
        };

        return new FairlightMixerMasterCommand(master);
    }
}
