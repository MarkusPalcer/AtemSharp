using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State;
using AtemSharp.State.Video.MixEffect;
using AtemSharp.State.Video.MixEffect.Transition;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class TransitionPropertiesUpdateCommandTests : DeserializedCommandTestBase<TransitionPropertiesUpdateCommand,
    TransitionPropertiesUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public TransitionStyle Style { get; set; }
        public TransitionSelection Selection { get; set; }
        public TransitionStyle NextStyle { get; set; }
        public TransitionSelection NextSelection { get; set; }
    }

    protected override void CompareCommandProperties(TransitionPropertiesUpdateCommand actualCommand,
                                                     CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.Style, Is.EqualTo(expectedData.Style));
        Assert.That(actualCommand.Selection, Is.EqualTo(expectedData.Selection));
        Assert.That(actualCommand.NextStyle, Is.EqualTo(expectedData.NextStyle));
        Assert.That(actualCommand.NextSelection, Is.EqualTo(expectedData.NextSelection));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Video.MixEffects = AtemStateUtil.CreateArray<MixEffect>(expectedData.Index + 1);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var actualCommand = state.Video.MixEffects[expectedData.Index].TransitionProperties;
        Assert.That(actualCommand.Style, Is.EqualTo(expectedData.Style));
        Assert.That(actualCommand.Selection, Is.EqualTo(expectedData.Selection));
        Assert.That(actualCommand.NextStyle, Is.EqualTo(expectedData.NextStyle));
        Assert.That(actualCommand.NextSelection, Is.EqualTo(expectedData.NextSelection));
    }
}
