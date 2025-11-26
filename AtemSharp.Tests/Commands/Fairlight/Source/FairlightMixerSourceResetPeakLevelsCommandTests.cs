using FairlightMixerSourceResetPeakLevelsCommand = AtemSharp.Commands.Audio.Fairlight.Source.FairlightMixerSourceResetPeakLevelsCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Source;

public class FairlightMixerSourceResetPeakLevelsCommandTests : SerializedCommandTestBase<FairlightMixerSourceResetPeakLevelsCommand, FairlightMixerSourceResetPeakLevelsCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
        public long SourceId { get; set; }
        public bool Output { get; set; }
        public bool DynamicsInput { get; set; }
        public bool DynamicsOutput { get; set; }
    }

    protected override FairlightMixerSourceResetPeakLevelsCommand CreateSut(TestCaseData testCase)
    {
        return new FairlightMixerSourceResetPeakLevelsCommand(new AtemSharp.State.Audio.Fairlight.Source
        {
            Id = testCase.Command.SourceId,
            InputId = testCase.Command.Index
        })
        {
            Output = testCase.Command.Output,
            DynamicsInput = testCase.Command.DynamicsInput,
            DynamicsOutput = testCase.Command.DynamicsOutput,
        };
    }
}
