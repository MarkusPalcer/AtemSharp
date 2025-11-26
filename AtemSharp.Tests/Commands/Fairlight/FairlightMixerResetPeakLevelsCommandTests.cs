using AtemSharp.Commands.Audio.Fairlight;

namespace AtemSharp.Tests.Commands.Fairlight;

[TestFixture]
public class FairlightMixerResetPeakLevelsCommandTests : SerializedCommandTestBase<FairlightMixerResetPeakLevelsCommand,
    FairlightMixerResetPeakLevelsCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool All { get; set; }
        public bool Master { get; set; }
    }

    protected override FairlightMixerResetPeakLevelsCommand CreateSut(TestCaseData testCase)
    {
        return new FairlightMixerResetPeakLevelsCommand
        {
            All = testCase.Command.All,
            Master = testCase.Command.Master,
        };
    }
}
