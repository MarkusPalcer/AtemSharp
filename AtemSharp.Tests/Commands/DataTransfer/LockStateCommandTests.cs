using AtemSharp.Commands.DataTransfer;

namespace AtemSharp.Tests.Commands.DataTransfer;

[TestFixture]
public class LockStateCommandTests : SerializedCommandTestBase<LockStateCommand, LockStateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
        public bool Locked { get; set; }
    }

    protected override LockStateCommand CreateSut(TestCaseData testCase)
    {
        // Create command with the test data values
        var command = new LockStateCommand
        {
            Index =testCase.Command.Index,
            Locked =testCase.Command.Locked
        };

        return command;
    }
}
