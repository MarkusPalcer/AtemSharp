using AtemSharp.State.Audio.Fairlight;
using FairlightMixerInputV8Command = AtemSharp.Commands.Audio.Fairlight.FairlightMixerInputV8Command;

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
        return new FairlightMixerInputV8Command(new FairlightAudioInput
        {
            Id = testCase.Command.Index,
            ActiveConfiguration = testCase.Command.ActiveConfiguration,
            ActiveInputLevel = testCase.Command.ActiveInputLevel
        });
    }
}
