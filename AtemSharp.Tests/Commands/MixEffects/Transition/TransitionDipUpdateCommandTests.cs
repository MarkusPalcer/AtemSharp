using AtemSharp.Commands.MixEffects.Transition;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class TransitionDipUpdateCommandTests : DeserializedCommandTestBase<TransitionDipUpdateCommand,
    TransitionDipUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public byte Rate { get; set; }
        public ushort Input { get; set; }
    }

    protected override void CompareCommandProperties(TransitionDipUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(testCase.Command.Index));
        Assert.That(actualCommand.Rate, Is.EqualTo(testCase.Command.Rate));
        Assert.That(actualCommand.Input, Is.EqualTo(testCase.Command.Input));
    }
}
