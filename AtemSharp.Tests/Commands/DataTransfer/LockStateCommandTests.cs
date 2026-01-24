using AtemSharp.Commands.DataTransfer;

namespace AtemSharp.Tests.Commands.DataTransfer;

[TestFixture]
public class LockStateCommandTests : TypeScriptLibrarySerializedCommandTestBase<LockStateCommand, LockStateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
        public bool Locked { get; set; }
    }

    protected override LockStateCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new LockStateCommand
        {
            Index =testCase.Command.Index,
            Locked =testCase.Command.Locked
        };
    }
}
