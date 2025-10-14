using AtemSharp.Commands.DataTransfer;

namespace AtemSharp.Tests.Commands.DataTransfer;

[TestFixture]
public class LockObtainedCommandTests : DeserializedCommandTestBase<LockObtainedCommand, LockObtainedCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
    }

    protected override void CompareCommandProperties(LockObtainedCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        var failures = new List<string>();

        // Compare Index - it is not floating point so it needs to equal exactly
        if (!actualCommand.Index.Equals(expectedData.Index))
        {
            failures.Add($"Index: expected {expectedData.Index}, actual {actualCommand.Index}");
        }

        // Assert results
        if (failures.Count > 0)
        {
            Assert.Fail($"Command deserialization property mismatch for version {testCase.FirstVersion}:\n" +
                       string.Join("\n", failures));
        }
    }
}
