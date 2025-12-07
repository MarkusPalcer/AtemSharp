using AtemSharp.Commands.Media;
using AtemSharp.State;
using AtemSharp.State.Media;

namespace AtemSharp.Tests.Commands.Media;

public class MediaPoolClipDescriptionCommandTests : DeserializedCommandTestBase<MediaPoolClipDescriptionCommand , MediaPoolClipDescriptionCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public bool IsUsed { get; set; }
        public string Name { get; set; } = string.Empty;
        public ushort FrameCount { get; set; }
    }

    protected override void CompareCommandProperties(MediaPoolClipDescriptionCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.ClipId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.IsUsed, Is.EqualTo(expectedData.IsUsed));
        Assert.That(actualCommand.Name, Is.EqualTo(expectedData.Name));
        Assert.That(actualCommand.FrameCount, Is.EqualTo(expectedData.FrameCount));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Media.Clips = AtemStateUtil.CreateArray<MediaPoolEntry>(expectedData.Index + 1);
    }


    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var actualCommand = state.Media.Clips[expectedData.Index];
        Assert.That(actualCommand.Id, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.IsUsed, Is.EqualTo(expectedData.IsUsed));
        Assert.That(actualCommand.Name, Is.EqualTo(expectedData.Name));
        Assert.That(actualCommand.FrameCount, Is.EqualTo(expectedData.FrameCount));
    }
}
