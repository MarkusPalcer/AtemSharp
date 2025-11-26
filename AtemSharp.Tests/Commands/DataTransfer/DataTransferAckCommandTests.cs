using AtemSharp.Commands.DataTransfer;

namespace AtemSharp.Tests.Commands.DataTransfer;

[TestFixture]
public class DataTransferAckCommandTests : SerializedCommandTestBase<DataTransferAckCommand,
    DataTransferAckCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort TransferId { get; set; }
        public byte TransferIndex { get; set; }
    }

    protected override DataTransferAckCommand CreateSut(TestCaseData testCase)
    {
        // Create command with the test data values
        return new DataTransferAckCommand(
            testCase.Command.TransferId,
            testCase.Command.TransferIndex);
    }
}
