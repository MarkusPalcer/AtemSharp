using AtemSharp.Commands.Fairlight;

namespace AtemSharp.Tests.Commands.Fairlight;

public class FairlightMixerMasterEqualizerResetCommandTests : SerializedCommandTestBase<FairlightMixerMasterEqualizerResetCommand, FairlightMixerMasterEqualizerResetCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool Equalizer { get; set; }
        public byte Band { get; set; }
    }

    protected override FairlightMixerMasterEqualizerResetCommand CreateSut(TestCaseData testCase)
    {
        return new FairlightMixerMasterEqualizerResetCommand()
        {
            Band = testCase.Command.Band,
            Equalizer = testCase.Command.Equalizer,
        };
    }
}
