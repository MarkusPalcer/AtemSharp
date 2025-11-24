using AtemSharp.Commands.SuperSource;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.SuperSource;

public class SuperSourcePropertiesUpdateV8CommandTests : DeserializedCommandTestBase<SuperSourcePropertiesUpdateV8Command, SuperSourcePropertiesUpdateV8CommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte SSrcId { get; set; }
        public ushort ArtFillSource { get; set; }
        public ushort ArtCutSource { get; set; }
        public ArtOption ArtOption { get; set; }
        public bool ArtPreMultiplied { get; set; }
        public double ArtClip { get; set; }
        public double ArtGain { get; set; }
        public bool ArtInvertKey { get; set; }
    }

    protected override void CompareCommandProperties(SuperSourcePropertiesUpdateV8Command actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.SuperSourceId, Is.EqualTo(expectedData.SSrcId));
        Assert.That(actualCommand.ArtFillSource, Is.EqualTo(expectedData.ArtFillSource));
        Assert.That(actualCommand.ArtCutSource, Is.EqualTo(expectedData.ArtCutSource));
        Assert.That(actualCommand.ArtOption, Is.EqualTo(expectedData.ArtOption));
        Assert.That(actualCommand.ArtPremultiplied, Is.EqualTo(expectedData.ArtPreMultiplied));
        Assert.That(actualCommand.ArtClip, Is.EqualTo(expectedData.ArtClip).Within(0.1));
        Assert.That(actualCommand.ArtGain, Is.EqualTo(expectedData.ArtGain).Within(0.1));
        Assert.That(actualCommand.ArtInvertKey,  Is.EqualTo(expectedData.ArtInvertKey));
    }
}
