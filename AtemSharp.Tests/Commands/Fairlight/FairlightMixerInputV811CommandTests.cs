using AtemSharp.Commands.Audio.Fairlight;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Tests.Commands.Fairlight;

public class FairlightMixerInputV811CommandTests : SerializedCommandTestBase<FairlightMixerInputV811Command,
    FairlightMixerInputV811CommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
        public FairlightInputConfiguration ActiveConfiguration { get; set; }
        public FairlightAnalogInputLevel ActiveInputLevel { get; set; }
    }

    protected override FairlightMixerInputV811Command CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new FairlightMixerInputV811Command(new FairlightAudioInput
        {
            Id = testCase.Command.Index,
            ActiveConfiguration = testCase.Command.ActiveConfiguration,
            ActiveInputLevel = testCase.Command.ActiveInputLevel
        });
    }
}
