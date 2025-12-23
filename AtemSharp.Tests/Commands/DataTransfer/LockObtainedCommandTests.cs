using AtemSharp.Commands.DataTransfer;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DataTransfer;

[TestFixture]
internal class LockObtainedCommandTests : DeserializedCommandTestBase<LockObtainedCommand, LockObtainedCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
    }

    internal override void CompareCommandProperties(LockObtainedCommand actualCommand, CommandData expectedData, TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        Assert.That(actualCommand.Index, Is.EqualTo(expectedData.Index));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        // No state change
    }
}
