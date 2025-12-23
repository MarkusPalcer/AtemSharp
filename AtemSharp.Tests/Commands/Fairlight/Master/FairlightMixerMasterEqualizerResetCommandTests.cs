using FairlightMixerMasterEqualizerResetCommand = AtemSharp.Commands.Audio.Fairlight.Master.FairlightMixerMasterEqualizerResetCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Master;

public class FairlightMixerMasterEqualizerResetCommandTests : SerializedCommandTestBase<FairlightMixerMasterEqualizerResetCommand, FairlightMixerMasterEqualizerResetCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool Equalizer { get; set; }
        public byte Band { get; set; }
    }

    protected override FairlightMixerMasterEqualizerResetCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new FairlightMixerMasterEqualizerResetCommand
        {
            Band = testCase.Command.Band,
            Equalizer = testCase.Command.Equalizer,
        };
    }
}
