using AtemSharp.Commands.Media;
using AtemSharp.State;
using AtemSharp.State.Media;

namespace AtemSharp.Tests.Commands.Media;

internal class MediaPoolFrameDescriptionCommandTests : DeserializedCommandTestBase<MediaPoolFrameDescriptionCommand,
    MediaPoolFrameDescriptionCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Bank { get; set; }
        public ushort Index { get; set; }
        public bool IsUsed { get; set; }
        public string Hash { get; set; } = string.Empty;
        public string Filename { get; set; } = string.Empty;
    }

    internal override void CompareCommandProperties(MediaPoolFrameDescriptionCommand actualCommand, CommandData expectedData, TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        Assert.That(actualCommand.MediaPoolId, Is.EqualTo((byte)expectedData.Bank));
        Assert.That(actualCommand.FrameIndex, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.IsUsed, Is.EqualTo(expectedData.IsUsed));
        Assert.That(actualCommand.Hash, Is.EqualTo(expectedData.Hash));
        Assert.That(actualCommand.FileName, Is.EqualTo(expectedData.Filename));
    }

    protected override bool TestApplyToState(CommandData testData)
    {
        return testData.Bank is 0 or 3;
    }


    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Media.Clips.GetOrCreate(expectedData.Index);
        state.Media.Frames.GetOrCreate(expectedData.Index);
    }


    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        MediaPoolEntry item = expectedData.Bank switch
        {
            0 => state.Media.Frames[expectedData.Index],
            3 => state.Media.Clips[expectedData.Index],
            _ => throw new ArgumentOutOfRangeException()
        };

        Assert.That(item.Id, Is.EqualTo(expectedData.Index));
        Assert.That(item.IsUsed, Is.EqualTo(expectedData.IsUsed));
        Assert.That(item.Hash, Is.EqualTo(expectedData.Hash));
        Assert.That(item.FileName, Is.EqualTo(expectedData.Filename));
    }
}
