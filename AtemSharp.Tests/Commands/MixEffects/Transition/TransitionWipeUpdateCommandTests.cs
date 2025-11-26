using AtemSharp.Commands.MixEffects.Transition;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class TransitionWipeUpdateCommandTests : DeserializedCommandTestBase<TransitionWipeUpdateCommand,
    TransitionWipeUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public byte Rate { get; set; }
        public byte Pattern { get; set; }
        public double BorderWidth { get; set; }
        public ushort BorderInput { get; set; }
        public double Symmetry { get; set; }
        public double BorderSoftness { get; set; }
        public double XPosition { get; set; }
        public double YPosition { get; set; }
        public bool ReverseDirection { get; set; }
        public bool FlipFlop { get; set; }
    }

    protected override void CompareCommandProperties(TransitionWipeUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(testCase.Command.Index));
        Assert.That(actualCommand.Rate, Is.EqualTo(expectedData.Rate));
        Assert.That(actualCommand.Pattern, Is.EqualTo(expectedData.Pattern));
        Assert.That(actualCommand.BorderWidth, Is.EqualTo(expectedData.BorderWidth).Within(0.01));
        Assert.That(actualCommand.BorderInput, Is.EqualTo(expectedData.BorderInput));
        Assert.That(actualCommand.Symmetry, Is.EqualTo(expectedData.Symmetry).Within(0.01));
        Assert.That(actualCommand.BorderSoftness, Is.EqualTo(expectedData.BorderSoftness).Within(0.01));
        Assert.That(actualCommand.XPosition, Is.EqualTo(expectedData.XPosition).Within(0.0001));
        Assert.That(actualCommand.YPosition, Is.EqualTo(expectedData.YPosition).Within(0.0001));
        Assert.That(actualCommand.ReverseDirection, Is.EqualTo(expectedData.ReverseDirection));
        Assert.That(actualCommand.FlipFlop, Is.EqualTo(expectedData.FlipFlop));
    }

}
