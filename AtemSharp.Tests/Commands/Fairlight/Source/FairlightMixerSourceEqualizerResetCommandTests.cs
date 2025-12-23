using FairlightMixerSourceEqualizerResetCommand = AtemSharp.Commands.Audio.Fairlight.Source.FairlightMixerSourceEqualizerResetCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Source;

public class FairlightMixerSourceEqualizerResetCommandTests : SerializedCommandTestBase<FairlightMixerSourceEqualizerResetCommand, FairlightMixerSourceEqualizerResetCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
        public long SourceId { get; set; }
        public bool Equalizer { get; set; }
        public byte Band { get; set; }
    }

    protected override FairlightMixerSourceEqualizerResetCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new FairlightMixerSourceEqualizerResetCommand(new AtemSharp.State.Audio.Fairlight.Source
        {
            Id = testCase.Command.SourceId,
            InputId = testCase.Command.Index,
        })
        {
            Equalizer = testCase.Command.Equalizer,
            Band = testCase.Command.Band
        };
    }
}
