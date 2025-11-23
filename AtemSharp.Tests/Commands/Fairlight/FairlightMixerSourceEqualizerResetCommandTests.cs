using AtemSharp.Commands.Fairlight;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Tests.Commands.Fairlight;

public class FairlightMixerSourceEqualizerResetCommandTests : SerializedCommandTestBase<FairlightMixerSourceEqualizerResetCommand, FairlightMixerSourceEqualizerResetCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
        public long SourceId { get; set; }
        public bool Equalizer { get; set; }
        public byte Band { get; set; }
    }

    protected override FairlightMixerSourceEqualizerResetCommand CreateSut(TestCaseData testCase)
    {
        return new FairlightMixerSourceEqualizerResetCommand(new Source
        {
            Id = testCase.Command.SourceId,
            InputId = testCase.Command.Index
        })
        {
            Equalizer = testCase.Command.Equalizer,
            Band = testCase.Command.Band
        };
    }
}
