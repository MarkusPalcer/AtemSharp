using AtemSharp.Commands.DeviceProfile;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DeviceProfile;

[TestFixture]
public class MacroPoolConfigCommandTests : DeserializedCommandTestBase<MacroPoolConfigCommand, MacroPoolConfigCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MacroCount { get; set; }
    }

    protected override void CompareCommandProperties(MacroPoolConfigCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.MacroCount, Is.EqualTo(expectedData.MacroCount));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.Info.MacroPool.MacroCount, Is.EqualTo(expectedData.MacroCount));
        Assert.That(state.Macros.Macros, Has.Length.EqualTo(expectedData.MacroCount));
    }
}
