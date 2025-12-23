using AtemSharp.Commands.Settings;
using AtemSharp.State;
using AtemSharp.State.Settings;

namespace AtemSharp.Tests.Commands.Settings;

[TestFixture]
public class TimeConfigCommandTests : SerializedCommandTestBase<TimeConfigCommand, TimeConfigCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public TimeMode Mode { get; set; }
    }

    protected override TimeConfigCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new TimeConfigCommand(new AtemState
        {
            Settings =
            {
                TimeMode = testCase.Command.Mode
            }
        });
    }
}
