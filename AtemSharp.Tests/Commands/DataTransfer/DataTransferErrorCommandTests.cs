using AtemSharp.Commands.DataTransfer;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DataTransfer;

[TestFixture]
internal class DataTransferErrorCommandTests : DeserializedCommandTestBase<DataTransferErrorCommand,
    DataTransferErrorCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort TransferId { get; set; }
        public ErrorCodes ErrorCode { get; set; }
    }

    internal override void CompareCommandProperties(DataTransferErrorCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.TransferId, Is.EqualTo(expectedData.TransferId));
        Assert.That(actualCommand.ErrorCode, Is.EqualTo(expectedData.ErrorCode));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        // No change in state
    }
}
