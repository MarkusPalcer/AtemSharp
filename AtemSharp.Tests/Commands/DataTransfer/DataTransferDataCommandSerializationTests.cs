using AtemSharp.Commands.DataTransfer;

namespace AtemSharp.Tests.Commands.DataTransfer;

[TestFixture]
public class DataTransferDataCommandSerializationTests : SerializedCommandTestBase<DataTransferDataCommand, DataTransferDataCommandSerializationTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort TransferId { get; set; }
        public string Body { get; set; } = "";
    }

    protected override DataTransferDataCommand CreateSut(TestCaseData testCase)
    {
        // Convert Base64 string back to byte array
        var bodyBytes = Convert.FromBase64String(testCase.Command.Body);

        // Create command with the test data values
        var command = new DataTransferDataCommand
        {
            TransferId = testCase.Command.TransferId,
            Body = bodyBytes
        };

        return command;
    }


}
