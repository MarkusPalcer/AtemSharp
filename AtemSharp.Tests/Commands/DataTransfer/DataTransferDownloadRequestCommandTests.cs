using AtemSharp.Commands.DataTransfer;

namespace AtemSharp.Tests.Commands.DataTransfer;

[TestFixture]
public class DataTransferDownloadRequestCommandTests : SerializedCommandTestBase<DataTransferDownloadRequestCommand,
    DataTransferDownloadRequestCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort TransferId { get; set; }
        public ushort TransferStoreId { get; set; }
        public ushort TransferIndex { get; set; }
        public ushort Unknown { get; set; }     // Maps to TransferType in our implementation
        public ushort Unknown2 { get; set; }    // Additional field not in TypeScript
    }

    protected override DataTransferDownloadRequestCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new DataTransferDownloadRequestCommand
        {
            TransferId = testCase.Command.TransferId,
            TransferStoreId = testCase.Command.TransferStoreId,
            TransferIndex = testCase.Command.TransferIndex,
            TransferType = testCase.Command.Unknown,
            Unknown2 = testCase.Command.Unknown2,
        };
    }
}
