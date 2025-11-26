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
        Assert.That(actualCommand.Index, Is.EqualTo(expectedData.Index));
    }
}
