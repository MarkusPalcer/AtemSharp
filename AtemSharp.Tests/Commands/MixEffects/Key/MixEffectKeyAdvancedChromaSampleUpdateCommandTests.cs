using AtemSharp.Commands.MixEffects.Key;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

[TestFixture]
public class MixEffectKeyAdvancedChromaSampleUpdateCommandTests : DeserializedCommandTestBase<MixEffectKeyAdvancedChromaSampleUpdateCommand, MixEffectKeyAdvancedChromaSampleUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MixEffectIndex { get; set; }
        public byte KeyerIndex { get; set; }
        public bool EnableCursor { get; set; }
        public bool Preview { get; set; }
        public double CursorX { get; set; }
        public double CursorY { get; set; }
        public double CursorSize { get; set; }
        public double SampledY { get; set; }
        public double SampledCb { get; set; }
        public double SampledCr { get; set; }
    }

    protected override void CompareCommandProperties(MixEffectKeyAdvancedChromaSampleUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.MixEffectIndex));
        Assert.That(actualCommand.KeyerId, Is.EqualTo(expectedData.KeyerIndex));
        Assert.That(actualCommand.EnableCursor, Is.EqualTo(expectedData.EnableCursor));
        Assert.That(actualCommand.Preview, Is.EqualTo(expectedData.Preview));
        Assert.That(actualCommand.CursorX, Is.EqualTo(expectedData.CursorX).Within(0.01));
        Assert.That(actualCommand.CursorY, Is.EqualTo(expectedData.CursorY).Within(0.01));
        Assert.That(actualCommand.CursorSize, Is.EqualTo(expectedData.CursorSize).Within(0.01));
        Assert.That(actualCommand.SampledY, Is.EqualTo(expectedData.SampledY).Within(0.0001));
        Assert.That(actualCommand.SampledCb, Is.EqualTo(expectedData.SampledCb).Within(0.0001));
        Assert.That(actualCommand.SampledCr, Is.EqualTo(expectedData.SampledCr).Within(0.0001));
    }
}
