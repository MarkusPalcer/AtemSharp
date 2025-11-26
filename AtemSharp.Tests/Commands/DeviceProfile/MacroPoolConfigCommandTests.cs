using AtemSharp.Commands.DeviceProfile;

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
}
