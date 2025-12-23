using FairlightMixerSourceCompressorCommand = AtemSharp.Commands.Audio.Fairlight.Source.FairlightMixerSourceCompressorCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Source;

public class FairlightMixerSourceCompressorCommandTests : SerializedCommandTestBase<FairlightMixerSourceCompressorCommand,
    FairlightMixerSourceCompressorCommandTests.CommandData>
{
    protected override Range[] GetFloatingPointByteRanges() =>
    [
        20..24, // Threshold
        24..26, // Ratio
        28..32, // Attack
        32..36, // Hold
        36..40  // Release
    ];

    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
        public long SourceId { get; set; }
        public bool CompressorEnabled { get; set; }
        public double Threshold { get; set; }
        public double Ratio { get; set; }
        public double Attack { get; set; }
        public double Hold { get; set; }
        public double Release { get; set; }
    }

    protected override FairlightMixerSourceCompressorCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new FairlightMixerSourceCompressorCommand(new AtemSharp.State.Audio.Fairlight.Source
        {
            Id = testCase.Command.SourceId,
            InputId = testCase.Command.Index,
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
