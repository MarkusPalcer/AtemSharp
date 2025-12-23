using FairlightMixerSourceExpanderCommand = AtemSharp.Commands.Audio.Fairlight.Source.FairlightMixerSourceExpanderCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Source;

public class FairlightMixerSourceExpanderCommandTests : SerializedCommandTestBase<FairlightMixerSourceExpanderCommand, FairlightMixerSourceExpanderCommandTests.CommandData>
{
    protected override Range[] GetFloatingPointByteRanges() => [
        (20..24), // Threshold
        (24..26), // Range
        (26..28), // Ratio
        (28..32), // Attack
        (32..36), // Hold
        (36..40)  // Release
    ];

    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
        public long SourceId { get; set; }
        public bool ExpanderEnabled { get; set; }
        public bool GateEnabled { get; set; }
        public double Threshold { get; set; }
        public double Range { get; set; }
        public double Ratio { get; set; }
        public double Attack { get; set; }
        public double Hold { get; set; }
        public double Release { get; set; }
    }

    protected override FairlightMixerSourceExpanderCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new FairlightMixerSourceExpanderCommand(new AtemSharp.State.Audio.Fairlight.Source
        {
            Id = testCase.Command.SourceId,
            InputId = testCase.Command.Index,
            Dynamics =
            {
                Expander =
                {
                    Enabled = testCase.Command.ExpanderEnabled,
                    GateEnabled = testCase.Command.GateEnabled,
                    Threshold = testCase.Command.Threshold,
                    Range = testCase.Command.Range,
                    Ratio = testCase.Command.Ratio,
                    Attack = testCase.Command.Attack,
                    Hold = testCase.Command.Hold,
                    Release = testCase.Command.Release
                }
            }
        });
    }
}
