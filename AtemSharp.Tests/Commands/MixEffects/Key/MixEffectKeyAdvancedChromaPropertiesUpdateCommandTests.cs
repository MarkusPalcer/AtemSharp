using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

[TestFixture]
internal class MixEffectKeyAdvancedChromaPropertiesUpdateCommandTests : DeserializedCommandTestBase<
    MixEffectKeyAdvancedChromaPropertiesUpdateCommand, MixEffectKeyAdvancedChromaPropertiesUpdateCommandTests.CommandData>
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

    internal override void CompareCommandProperties(MixEffectKeyAdvancedChromaPropertiesUpdateCommand actualCommand,
                                                     CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.MixEffectIndex));
        Assert.That(actualCommand.KeyerId, Is.EqualTo(expectedData.KeyerIndex));
        Assert.That(actualCommand.ForegroundLevel, Is.EqualTo(expectedData.ForegroundLevel).Within(0.1));
        Assert.That(actualCommand.BackgroundLevel, Is.EqualTo(expectedData.BackgroundLevel).Within(0.1));
        Assert.That(actualCommand.KeyEdge, Is.EqualTo(expectedData.KeyEdge).Within(0.1));
        Assert.That(actualCommand.SpillSuppression, Is.EqualTo(expectedData.SpillSuppression).Within(0.1));
        Assert.That(actualCommand.FlareSuppression, Is.EqualTo(expectedData.FlareSuppression).Within(0.1));
        Assert.That(actualCommand.Brightness, Is.EqualTo(expectedData.Brightness).Within(0.1));
        Assert.That(actualCommand.Contrast, Is.EqualTo(expectedData.Contrast).Within(0.1));
        Assert.That(actualCommand.Saturation, Is.EqualTo(expectedData.Saturation).Within(0.1));
        Assert.That(actualCommand.Red, Is.EqualTo(expectedData.Red).Within(0.1));
        Assert.That(actualCommand.Green, Is.EqualTo(expectedData.Green).Within(0.1));
        Assert.That(actualCommand.Blue, Is.EqualTo(expectedData.Blue).Within(0.1));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Video.MixEffects.GetOrCreate(expectedData.MixEffectIndex);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var actualCommand = state.Video.MixEffects[expectedData.MixEffectIndex].UpstreamKeyers[expectedData.KeyerIndex]
                                 .AdvancedChromaSettings.Properties;
        Assert.That(actualCommand.ForegroundLevel, Is.EqualTo(expectedData.ForegroundLevel).Within(0.1));
        Assert.That(actualCommand.BackgroundLevel, Is.EqualTo(expectedData.BackgroundLevel).Within(0.1));
        Assert.That(actualCommand.KeyEdge, Is.EqualTo(expectedData.KeyEdge).Within(0.1));
        Assert.That(actualCommand.SpillSuppression, Is.EqualTo(expectedData.SpillSuppression).Within(0.1));
        Assert.That(actualCommand.FlareSuppression, Is.EqualTo(expectedData.FlareSuppression).Within(0.1));
        Assert.That(actualCommand.Brightness, Is.EqualTo(expectedData.Brightness).Within(0.1));
        Assert.That(actualCommand.Contrast, Is.EqualTo(expectedData.Contrast).Within(0.1));
        Assert.That(actualCommand.Saturation, Is.EqualTo(expectedData.Saturation).Within(0.1));
        Assert.That(actualCommand.Red, Is.EqualTo(expectedData.Red).Within(0.1));
        Assert.That(actualCommand.Green, Is.EqualTo(expectedData.Green).Within(0.1));
        Assert.That(actualCommand.Blue, Is.EqualTo(expectedData.Blue).Within(0.1));
    }
}
