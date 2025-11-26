using AtemSharp.Commands.DataTransfer;

namespace AtemSharp.Tests.Commands.DataTransfer;

[TestFixture]
public class DataTransferErrorCommandTests : DeserializedCommandTestBase<DataTransferErrorCommand, DataTransferErrorCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort TransferId { get; set; }
        public DataTransferErrorCommand.ErrorCodes ErrorCode { get; set; }
    }

    protected override void CompareCommandProperties(DataTransferErrorCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.TransferId, Is.EqualTo(expectedData.TransferId));
        Assert.That(actualCommand.ErrorCode, Is.EqualTo(expectedData.ErrorCode));
    }
}
