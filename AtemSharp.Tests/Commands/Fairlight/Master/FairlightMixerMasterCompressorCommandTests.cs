using AtemSharp.State.Audio.Fairlight;
using FairlightMixerMasterCompressorCommand = AtemSharp.Commands.Audio.Fairlight.Master.FairlightMixerMasterCompressorCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Master;

public class FairlightMixerMasterCompressorCommandTests : SerializedCommandTestBase<FairlightMixerMasterCompressorCommand, FairlightMixerMasterCompressorCommandTests.CommandData>
{
    protected override Range[] GetFloatingPointByteRanges() => [
        (4..8), // Threshold
        (8..10), // Ratio
        (12..16), // Attack
        (16..20), // Hold
        (20..24)  // Release
    ];

    public class CommandData : CommandDataBase
    {
        public bool CompressorEnabled { get; set; }
        public double Threshold { get; set; }
        public double Ratio { get; set; }
        public double Attack { get; set; }
        public double Hold { get; set; }
        public double Release { get; set; }
    }

    protected override FairlightMixerMasterCompressorCommand CreateSut(TestCaseData testCase)
    {
        return new FairlightMixerMasterCompressorCommand(new MasterProperties
        {
            Dynamics =
            {
                Compressor =
                {
                    Enabled = testCase.Command.CompressorEnabled,
                    Threshold = testCase.Command.Threshold,
                    Ratio = testCase.Command.Ratio,
                    Attack = testCase.Command.Attack,
                    Hold = testCase.Command.Hold,
                    Release = testCase.Command.Release,
                }
            }
        });
    }
}
