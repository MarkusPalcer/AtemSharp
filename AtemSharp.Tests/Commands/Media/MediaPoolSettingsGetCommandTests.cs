using AtemSharp.Commands.Media;

namespace AtemSharp.Tests.Commands.Media;

public class MediaPoolSettingsGetCommandTests : DeserializedCommandTestBase<MediaPoolSettingsGetCommand, MediaPoolSettingsGetCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort[] MaxFrames { get; set; } = [];
        public ushort UnassignedFrames { get; set; }
    }

    protected override void CompareCommandProperties(MediaPoolSettingsGetCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.MaxFrames, Is.EqualTo(expectedData.MaxFrames));
        Assert.That(actualCommand.UnassignedFrames, Is.EqualTo(expectedData.UnassignedFrames));
    }
}
