using AtemSharp.Commands.MixEffects.Key;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

[TestFixture]
public class MixEffectKeyAdvancedChromaPropertiesUpdateCommandTests : DeserializedCommandTestBase<MixEffectKeyAdvancedChromaPropertiesUpdateCommand, MixEffectKeyAdvancedChromaPropertiesUpdateCommandTests.CommandData>
{

    public class CommandData : CommandDataBase
    {
        public byte MixEffectIndex { get; set; }
        public byte KeyerIndex { get; set; }
        public double ForegroundLevel { get; set; }
        public double BackgroundLevel { get; set; }
        public double KeyEdge { get; set; }
        public double SpillSuppression { get; set; }
        public double FlareSuppression { get; set; }
        public double Brightness { get; set; }
        public double Contrast { get; set; }
        public double Saturation { get; set; }
        public double Red { get; set; }
        public double Green { get; set; }
        public double Blue { get; set; }
    }

    protected override void CompareCommandProperties(MixEffectKeyAdvancedChromaPropertiesUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(testCase.Command.MixEffectIndex));
        Assert.That(actualCommand.KeyerId, Is.EqualTo(testCase.Command.KeyerIndex));
        Assert.That(actualCommand.ForegroundLevel, Is.EqualTo(testCase.Command.ForegroundLevel).Within(0.1));
        Assert.That(actualCommand.BackgroundLevel, Is.EqualTo(testCase.Command.BackgroundLevel).Within(0.1));
        Assert.That(actualCommand.KeyEdge, Is.EqualTo(testCase.Command.KeyEdge).Within(0.1));
        Assert.That(actualCommand.SpillSuppression, Is.EqualTo(testCase.Command.SpillSuppression).Within(0.1));
        Assert.That(actualCommand.FlareSuppression, Is.EqualTo(testCase.Command.FlareSuppression).Within(0.1));
        Assert.That(actualCommand.Brightness, Is.EqualTo(testCase.Command.Brightness).Within(0.1));
        Assert.That(actualCommand.Contrast, Is.EqualTo(testCase.Command.Contrast).Within(0.1));
        Assert.That(actualCommand.Saturation, Is.EqualTo(testCase.Command.Saturation).Within(0.1));
        Assert.That(actualCommand.Red, Is.EqualTo(testCase.Command.Red).Within(0.1));
        Assert.That(actualCommand.Green, Is.EqualTo(testCase.Command.Green).Within(0.1));
        Assert.That(actualCommand.Blue, Is.EqualTo(testCase.Command.Blue).Within(0.1));
    }
}
