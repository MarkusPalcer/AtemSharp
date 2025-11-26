using AtemSharp.Commands.DeviceProfile;

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
}
