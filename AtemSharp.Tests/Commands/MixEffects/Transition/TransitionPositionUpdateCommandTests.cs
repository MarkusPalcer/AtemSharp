using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
internal class TransitionPositionUpdateCommandTests : DeserializedCommandTestBase<TransitionPositionUpdateCommand,
    TransitionPositionUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public bool InTransition { get; set; }
        public byte RemainingFrames { get; set; }
        public double HandlePosition { get; set; }
    }

    internal override void CompareCommandProperties(TransitionPositionUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.InTransition, Is.EqualTo(expectedData.InTransition));
        Assert.That(actualCommand.RemainingFrames, Is.EqualTo(expectedData.RemainingFrames));
        Assert.That(actualCommand.HandlePosition, Is.EqualTo(expectedData.HandlePosition).Within(0.0001));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Video.MixEffects.GetOrCreate(expectedData.Index);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var actualCommand = state.Video.MixEffects[expectedData.Index].TransitionPosition;
        Assert.That(actualCommand.InTransition, Is.EqualTo(expectedData.InTransition));
        Assert.That(actualCommand.RemainingFrames, Is.EqualTo(expectedData.RemainingFrames));
        Assert.That(actualCommand.HandlePosition, Is.EqualTo(expectedData.HandlePosition).Within(0.0001));
    }
}
