using AtemSharp.State.Audio.Fairlight;
using FairlightMixerMonitorCommand = AtemSharp.Commands.Audio.Fairlight.Monitor.FairlightMixerMonitorCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Monitor;

public class FairlightMixerMonitorCommandTests : SerializedCommandTestBase<FairlightMixerMonitorCommand, FairlightMixerMonitorCommandTests.CommandData>
{
    protected override Range[] GetFloatingPointByteRanges() =>
    [
        (4..8),    // Gain
        (8..12),  // InputMasterGain
        (16..20), // InputTalkbackGain
        (32..36)  // InputSidetoneGain
    ];

    public class CommandData : CommandDataBase
    {
        public double Gain { get; set; }
        public double InputMasterGain { get; set; }
        public bool InputMasterEnabled { get; set; }
        public double InputTalkbackGain { get; set; }
        public double InputSidetoneGain { get; set; }
    }

    protected override FairlightMixerMonitorCommand CreateSut(TestCaseData testCase)
    {
        return new FairlightMixerMonitorCommand(new FairlightAudioState
        {
            Monitor =
            {
                Gain = testCase.Command.Gain,
                InputMasterGain = testCase.Command.InputMasterGain,
                InputMasterMuted = testCase.Command.InputMasterEnabled,
                InputTalkbackGain = testCase.Command.InputTalkbackGain,
                InputSidetoneGain = testCase.Command.InputSidetoneGain
            }
        });
    }
}
