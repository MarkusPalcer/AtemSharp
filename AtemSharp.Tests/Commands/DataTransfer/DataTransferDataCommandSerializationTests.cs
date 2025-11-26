using AtemSharp.Commands.DataTransfer;

namespace AtemSharp.Tests.Commands.DataTransfer;

[TestFixture]
public class DataTransferDataCommandSerializationTests : SerializedCommandTestBase<DataTransferDataCommand,
    DataTransferDataCommandSerializationTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort TransferId { get; set; }
        public string Body { get; set; } = "";
    }

    protected override DataTransferDataCommand CreateSut(TestCaseData testCase)
    {
        return new DataTransferDataCommand
        {
            TransferId = testCase.Command.TransferId,
            Body = Convert.FromBase64String(testCase.Command.Body)
        };
    }
}
