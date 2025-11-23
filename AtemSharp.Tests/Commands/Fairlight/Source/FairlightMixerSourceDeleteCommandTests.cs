using FairlightMixerSourceDeleteCommand = AtemSharp.Commands.Fairlight.Source.FairlightMixerSourceDeleteCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Source;

public class FairlightMixerSourceDeleteCommandTests : DeserializedCommandTestBase<FairlightMixerSourceDeleteCommand, FairlightMixerSourceDeleteCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Index { get; set; }
        public string SourceId { get; set; } = string.Empty;
    }

    protected override void CompareCommandProperties(FairlightMixerSourceDeleteCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.InputId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.SourceId.ToString(), Is.EqualTo(expectedData.SourceId));
    }
}
