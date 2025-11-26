using AtemSharp.Commands.MixEffects.Transition;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class PreviewTransitionUpdateCommandTests : DeserializedCommandTestBase<PreviewTransitionUpdateCommand,
    PreviewTransitionUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Index { get; set; }
        public bool PreviewTransition { get; set; }
    }

    protected override void CompareCommandProperties(PreviewTransitionUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.Preview, Is.EqualTo(expectedData.PreviewTransition));
    }
}
