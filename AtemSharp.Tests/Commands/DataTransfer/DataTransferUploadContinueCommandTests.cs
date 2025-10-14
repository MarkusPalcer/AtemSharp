using AtemSharp.Commands.DataTransfer;

namespace AtemSharp.Tests.Commands.DataTransfer;

[TestFixture]
public class DataTransferUploadContinueCommandTests : DeserializedCommandTestBase<DataTransferUploadContinueCommand,
    DataTransferUploadContinueCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort TransferId { get; set; }
        public ushort ChunkSize { get; set; }
        public ushort ChunkCount { get; set; }
    }

    protected override void CompareCommandProperties(DataTransferUploadContinueCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        var failures = new List<string>();

        // Compare TransferId - it is not floating point so it needs to equal exactly
        if (!actualCommand.TransferId.Equals(expectedData.TransferId))
        {
            failures.Add($"TransferId: expected {expectedData.TransferId}, actual {actualCommand.TransferId}");
        }

        // Compare ChunkSize - it is not floating point so it needs to equal exactly
        if (!actualCommand.ChunkSize.Equals(expectedData.ChunkSize))
        {
            failures.Add($"ChunkSize: expected {expectedData.ChunkSize}, actual {actualCommand.ChunkSize}");
        }

        // Compare ChunkCount - it is not floating point so it needs to equal exactly
        if (!actualCommand.ChunkCount.Equals(expectedData.ChunkCount))
        {
            failures.Add($"ChunkCount: expected {expectedData.ChunkCount}, actual {actualCommand.ChunkCount}");
        }

        // Assert results
        if (failures.Count > 0)
        {
            Assert.Fail($"Command deserialization property mismatch for version {testCase.FirstVersion}:\n" +
                        string.Join("\n", failures));
        }
    }

}
