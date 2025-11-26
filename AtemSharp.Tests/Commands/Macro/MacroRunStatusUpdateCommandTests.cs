using AtemSharp.Commands.Macro;

namespace AtemSharp.Tests.Commands.Macro;

public class MacroRunStatusUpdateCommandTests : DeserializedCommandTestBase<MacroRunStatusUpdateCommand, MacroRunStatusUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool IsRunning { get; set; }
        public bool Loop { get; set; }
        public bool IsWaiting { get; set; }
        public ushort Index { get; set; }
    }

    protected override void CompareCommandProperties(MacroRunStatusUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.IsRunning, Is.EqualTo(expectedData.IsRunning));
        Assert.That(actualCommand.Loop, Is.EqualTo(expectedData.Loop));
        Assert.That(actualCommand.IsWaiting, Is.EqualTo(expectedData.IsWaiting));
        Assert.That(actualCommand.MacroIndex, Is.EqualTo(expectedData.Index));
    }
}
