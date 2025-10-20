using AtemSharp.Commands.Macro;

namespace AtemSharp.Tests.Commands.Macro;

public class MacroPropertiesUpdateCommandTests : DeserializedCommandTestBase<MacroPropertiesUpdateCommand, MacroPropertiesUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
        public bool IsUsed { get; set; }
        public bool HasUnsupportedOps { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    protected override void CompareCommandProperties(MacroPropertiesUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.Id, Is.EqualTo(expectedData.Index), $"{testCase}: Id");
        Assert.That(actualCommand.IsUsed, Is.EqualTo(expectedData.IsUsed), $"{testCase}: IsUsed");
        Assert.That(actualCommand.HasUnsupportedOps, Is.EqualTo(expectedData.HasUnsupportedOps), $"{testCase}: HasUnsupportedOps");
        Assert.That(actualCommand.Name, Is.EqualTo(expectedData.Name), $"{testCase}: Name");
        Assert.That(actualCommand.Description, Is.EqualTo(expectedData.Description), $"{testCase}: Description");
    }
}
