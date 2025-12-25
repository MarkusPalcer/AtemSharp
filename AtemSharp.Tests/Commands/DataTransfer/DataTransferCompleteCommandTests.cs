using AtemSharp.Commands.DataTransfer;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DataTransfer;

[TestFixture]
internal class DataTransferCompleteCommandTests : DeserializedCommandTestBase<DataTransferCompleteCommand,
    DataTransferCompleteCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort TransferId { get; set; }
    }

    internal override void CompareCommandProperties(DataTransferCompleteCommand actualCommand, CommandData expectedData)
    {
        Assert.That(actualCommand.TransferId, Is.EqualTo(expectedData.TransferId));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        // No state changes for data transfers
    }
}
