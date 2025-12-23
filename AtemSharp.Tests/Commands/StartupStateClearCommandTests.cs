using AtemSharp.Commands.StartupState;

namespace AtemSharp.Tests.Commands;

[TestFixture]
public class StartupStateClearCommandTests : SerializedCommandTestBase<StartupStateClearCommand, StartupStateClearCommandTests.CommandData>
{
    public class CommandData : CommandDataBase;

    protected override StartupStateClearCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new StartupStateClearCommand();
    }
}
