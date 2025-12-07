using AtemSharp.Commands.Media;
using AtemSharp.State;
using AtemSharp.State.Media;

namespace AtemSharp.Tests.Commands.Media;

public class MediaPoolFrameDescriptionCommandTests : DeserializedCommandTestBase<MediaPoolFrameDescriptionCommand,
    MediaPoolFrameDescriptionCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Bank { get; set; }
        public int Index { get; set; }
        public bool IsUsed { get; set; }
        public string Hash { get; set; } = string.Empty;
        public string Filename { get; set; } = string.Empty;
    }

    protected override void CompareCommandProperties(MediaPoolFrameDescriptionCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.MediaPoolId, Is.EqualTo((byte)expectedData.Bank));
        Assert.That(actualCommand.FrameIndex, Is.EqualTo((ushort)expectedData.Index));
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
        state.Media.Clips = AtemStateUtil.CreateArray<MediaPoolEntry>(expectedData.Index + 1);
        state.Media.Frames = AtemStateUtil.CreateArray<MediaPoolEntry>(expectedData.Index + 1);
    }


    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var source = expectedData.Bank switch
        {
            0 => state.Media.Frames,
            3 => state.Media.Clips,
            _ => throw new ArgumentOutOfRangeException()
        };

        var item = source[expectedData.Index];
        Assert.That(item.Id, Is.EqualTo((byte)expectedData.Index));
        Assert.That(item.IsUsed, Is.EqualTo(expectedData.IsUsed));
        Assert.That(item.Hash, Is.EqualTo(expectedData.Hash));
        Assert.That(item.FileName, Is.EqualTo(expectedData.Filename));
    }
}
