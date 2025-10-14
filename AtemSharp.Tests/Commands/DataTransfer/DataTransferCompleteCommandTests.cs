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
        var failures = new List<string>();

        // Compare TransferId - it is not floating point so it needs to equal exactly
        if (!actualCommand.TransferId.Equals(expectedData.TransferId))
        {
            failures.Add($"TransferId: expected {expectedData.TransferId}, actual {actualCommand.TransferId}");
        }

        // Assert results
        if (failures.Count > 0)
        {
            Assert.Fail($"Command deserialization property mismatch for version {testCase.FirstVersion}:\n" +
                       string.Join("\n", failures));
        }
    }
}
