using AtemSharp.Commands.Fairlight;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Tests.Commands.Fairlight;

public class FairlightMixerInputV8CommandTests : SerializedCommandTestBase<FairlightMixerInputV8Command,
    FairlightMixerInputV8CommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
        public FairlightInputConfiguration ActiveConfiguration { get; set; }
        public FairlightAnalogInputLevel ActiveInputLevel { get; set; }
    }

    protected override FairlightMixerInputV8Command CreateSut(TestCaseData testCase)
    {
        var input = new FairlightAudioInput
        {
            Id = testCase.Command.Index,
            ActiveConfiguration = testCase.Command.ActiveConfiguration,
            ActiveInputLevel = testCase.Command.ActiveInputLevel
        };

        return new FairlightMixerInputV8Command(input);
    }
}
