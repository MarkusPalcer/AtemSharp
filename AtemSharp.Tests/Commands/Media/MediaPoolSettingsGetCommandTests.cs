using AtemSharp.Commands.Media;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Media;

internal class MediaPoolSettingsGetCommandTests : DeserializedCommandTestBase<MediaPoolSettingsGetCommand, MediaPoolSettingsGetCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort[] MaxFrames { get; set; } = [];
        public ushort UnassignedFrames { get; set; }
    }

    internal override void CompareCommandProperties(MediaPoolSettingsGetCommand actualCommand, CommandData expectedData)
    {
        Assert.That(actualCommand.MaxFrames, Is.EqualTo(expectedData.MaxFrames));
        Assert.That(actualCommand.UnassignedFrames, Is.EqualTo(expectedData.UnassignedFrames));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var actualCommand = state.Settings.MediaPool;
        Assert.That(actualCommand.MaxFrames, Is.EqualTo(expectedData.MaxFrames));
        Assert.That(actualCommand.UnassignedFrames, Is.EqualTo(expectedData.UnassignedFrames));
    }
}
