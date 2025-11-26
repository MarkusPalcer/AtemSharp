using AtemSharp.Commands.DataTransfer;

namespace AtemSharp.Tests.Commands.DataTransfer;

[TestFixture]
public class DataTransferUploadRequestCommandTests : SerializedCommandTestBase<DataTransferUploadRequestCommand,
    DataTransferUploadRequestCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort TransferId { get; set; }
        public ushort TransferStoreId { get; set; }
        public ushort TransferIndex { get; set; }
        public uint Size { get; set; }
        public ushort Mode { get; set; }
    }

    protected override DataTransferUploadRequestCommand CreateSut(TestCaseData testCase)
    {
        return new DataTransferUploadRequestCommand(
            testCase.Command.TransferId,
            testCase.Command.TransferStoreId,
            testCase.Command.TransferIndex,
            testCase.Command.Size,
            testCase.Command.Mode);
    }
}
