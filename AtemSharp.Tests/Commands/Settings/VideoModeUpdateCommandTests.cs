using AtemSharp.Commands.Settings;
using AtemSharp.State.Settings;

namespace AtemSharp.Tests.Commands.Settings;

/// <summary>
/// Manual tests for VideoModeUpdateCommand since test data from libatem-data.json
/// contains inconsistent expected values for the mode property.
/// All test cases have Mode=N525i5994NTSC but expect different byte inputs.
/// </summary>
[TestFixture]
public class VideoModeUpdateCommandTests : DeserializedCommandTestBase<VideoModeUpdateCommand, VideoModeUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public VideoMode VideoMode { get; set; }
    }

    protected override void CompareCommandProperties(VideoModeUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.Mode, Is.EqualTo(expectedData.VideoMode));
    }
}
