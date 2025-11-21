using AtemSharp.Commands.StartupState;

namespace AtemSharp.Tests.Commands;

[TestFixture]
public class StartupStateSaveCommandTests : SerializedCommandTestBase<StartupStateSaveCommand, StartupStateSaveCommandTests.CommandData>
{
    public class CommandData : CommandDataBase {}

    protected override StartupStateSaveCommand CreateSut(TestCaseData testCase)
    {
        return new StartupStateSaveCommand();
    }
}
