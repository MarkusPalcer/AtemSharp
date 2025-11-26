using AtemSharp.Commands.MixEffects;

namespace AtemSharp.Tests.Commands.MixEffects;

[TestFixture]
public class PreviewInputUpdateCommandTests : DeserializedCommandTestBase<PreviewInputUpdateCommand,
    PreviewInputUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Index { get; set; }
        public int Source { get; set; }
    }

    protected override void CompareCommandProperties(PreviewInputUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.Source, Is.EqualTo(expectedData.Source));
    }

}
