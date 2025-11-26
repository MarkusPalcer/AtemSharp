using AtemSharp.Commands.MixEffects.Transition;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class TransitionPositionUpdateCommandTests : DeserializedCommandTestBase<TransitionPositionUpdateCommand,
    TransitionPositionUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public bool InTransition { get; set; }
        public byte RemainingFrames { get; set; }
        public double HandlePosition { get; set; }
    }

    protected override void CompareCommandProperties(TransitionPositionUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.InTransition, Is.EqualTo(expectedData.InTransition));
        Assert.That(actualCommand.RemainingFrames, Is.EqualTo(expectedData.RemainingFrames));
        Assert.That(actualCommand.HandlePosition, Is.EqualTo(expectedData.HandlePosition).Within(0.0001));
    }
}
