using AtemSharp.Commands.DeviceProfile;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DeviceProfile;

[TestFixture]
public class MediaPoolConfigCommandTests : DeserializedCommandTestBase<MediaPoolConfigCommand, MediaPoolConfigCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte StillCount { get; set; }
        public byte ClipCount { get; set; }
    }

    protected override void CompareCommandProperties(MediaPoolConfigCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.StillCount, Is.EqualTo(expectedData.StillCount));
        Assert.That(actualCommand.ClipCount, Is.EqualTo(expectedData.ClipCount));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.Info.MediaPool.StillCount, Is.EqualTo(expectedData.StillCount));
        Assert.That(state.Info.MediaPool.ClipCount, Is.EqualTo(expectedData.ClipCount));
        Assert.That(state.Media.Frames.Count(), Is.EqualTo(expectedData.StillCount));
        Assert.That(state.Media.Clips.Count(), Is.EqualTo(expectedData.ClipCount));
    }
}
