using AtemSharp.Commands.DataTransfer;

namespace AtemSharp.Tests.Commands.DataTransfer;

[TestFixture]
public class DataTransferCompleteCommandTests : DeserializedCommandTestBase<DataTransferCompleteCommand, DataTransferCompleteCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort TransferId { get; set; }
    }

    protected override void CompareCommandProperties(DataTransferCompleteCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.TransferId, Is.EqualTo(expectedData.TransferId));
    }
}
