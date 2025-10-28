using AtemSharp.Commands.Settings;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Settings;

/// <summary>
/// Manual tests for VideoModeUpdateCommand since test data from libatem-data.json
/// contains inconsistent expected values for the mode property.
/// All test cases have Mode=N525i5994NTSC but expect different byte inputs.
/// </summary>
[TestFixture]
// TODO: Check test data
public class VideoModeUpdateCommandTests : DeserializedCommandTestBase<VideoModeUpdateCommand, VideoModeUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public VideoMode VideoMode { get; set; }
    }

    [Test]
    public void ApplyToState_ShouldUpdateVideoMode()
    {
        // Arrange
        var state = new AtemState
        {
            Settings = new SettingsState
            {
                VideoMode = VideoMode.N525i5994NTSC
            }
        };

        var command = new VideoModeUpdateCommand
        {
            Mode = VideoMode.N1080p5994
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Settings.VideoMode, Is.EqualTo(VideoMode.N1080p5994),
                   "State should be updated with new video mode");
    }

    protected override void CompareCommandProperties(VideoModeUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.Mode, Is.EqualTo(expectedData.VideoMode));
    }
}
