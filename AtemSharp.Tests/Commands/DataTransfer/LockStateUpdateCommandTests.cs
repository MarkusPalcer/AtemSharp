using AtemSharp.Commands.DataTransfer;

namespace AtemSharp.Tests.Commands.DataTransfer;

[TestFixture]
public class LockStateUpdateCommandTests : DeserializedCommandTestBase<LockStateUpdateCommand, LockStateUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
        public bool Locked { get; set; }
    }

    protected override void CompareCommandProperties(LockStateUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.Index, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.Locked, Is.EqualTo(expectedData.Locked));
    }
}
