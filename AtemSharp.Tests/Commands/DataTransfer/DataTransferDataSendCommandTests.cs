using AtemSharp.Commands.DataTransfer;

namespace AtemSharp.Tests.Commands.DataTransfer;

[TestFixture]
public class DataTransferDataSendCommandTests : SerializedCommandTestBase<DataTransferDataSendCommand,
    DataTransferDataSendCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort TransferId { get; set; }
        public string Body { get; set; } = "";
    }

    protected override DataTransferDataSendCommand CreateSut(TestCaseData testCase)
    {
        return new DataTransferDataSendCommand
        {
            TransferId = testCase.Command.TransferId,
            Body = Convert.FromBase64String(testCase.Command.Body)
        };
    }
}
