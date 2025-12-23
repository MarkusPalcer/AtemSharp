using AtemSharp.Commands.DataTransfer;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DataTransfer;

[TestFixture]
internal class DataTransferUploadContinueCommandTests : DeserializedCommandTestBase<DataTransferUploadContinueCommand,
    DataTransferUploadContinueCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort TransferId { get; set; }
        public ushort ChunkSize { get; set; }
        public ushort ChunkCount { get; set; }
    }

    internal override void CompareCommandProperties(DataTransferUploadContinueCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.TransferId, Is.EqualTo(expectedData.TransferId));
        Assert.That(actualCommand.ChunkSize, Is.EqualTo(expectedData.ChunkSize));
        Assert.That(actualCommand.ChunkCount, Is.EqualTo(expectedData.ChunkCount));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        // No change in state
    }
}
