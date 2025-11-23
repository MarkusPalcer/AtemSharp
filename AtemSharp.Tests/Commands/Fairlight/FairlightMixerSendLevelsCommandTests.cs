using AtemSharp.Commands.Fairlight;

namespace AtemSharp.Tests.Commands.Fairlight;

public class FairlightMixerSendLevelsCommandTests : SerializedCommandTestBase<FairlightMixerSendLevelsCommand, FairlightMixerSendLevelsCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool SendLevels  { get; set; }
    }

    protected override FairlightMixerSendLevelsCommand CreateSut(TestCaseData testCase)
    {
        return new FairlightMixerSendLevelsCommand
        {
            SendLevels = testCase.Command.SendLevels
        };
    }
}
