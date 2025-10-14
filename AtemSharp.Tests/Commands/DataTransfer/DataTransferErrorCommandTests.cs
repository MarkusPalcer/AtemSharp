using AtemSharp.Commands.DataTransfer;
using AtemSharp.Enums.DataTransfer;

namespace AtemSharp.Tests.Commands.DataTransfer;

[TestFixture]
public class DataTransferErrorCommandTests : DeserializedCommandTestBase<DataTransferErrorCommand, DataTransferErrorCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort TransferId { get; set; }
        public ErrorCode ErrorCode { get; set; }
    }

    protected override void CompareCommandProperties(DataTransferErrorCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        var failures = new List<string>();

        // Compare TransferId - it is not floating point so it needs to equal exactly
        if (!actualCommand.TransferId.Equals(expectedData.TransferId))
        {
            failures.Add($"TransferId: expected {expectedData.TransferId}, actual {actualCommand.TransferId}");
        }

        // Compare ErrorCode - enum values should match exactly
        if (!actualCommand.ErrorCode.Equals(expectedData.ErrorCode))
        {
            failures.Add($"ErrorCode: expected {expectedData.ErrorCode}, actual {actualCommand.ErrorCode}");
        }

        // Assert results
        if (failures.Count > 0)
        {
            Assert.Fail($"Command deserialization property mismatch for version {testCase.FirstVersion}:\n" +
                       string.Join("\n", failures));
        }
    }
}
