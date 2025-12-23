using AtemSharp.Commands.Settings;
using AtemSharp.State;
using AtemSharp.State.Settings;

namespace AtemSharp.Tests.Commands.Settings;

/// <summary>
/// Manual tests for VideoModeCommand since test data from libatem-data.json
/// contains inconsistent expected values for the mode property.
/// All test cases have Mode=N525i5994NTSC but expect different byte outputs.
/// </summary>
[TestFixture]
public class VideoModeCommandTests : SerializedCommandTestBase<VideoModeCommand, VideoModeCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public VideoMode VideoMode { get; set; }
    }

    protected override VideoModeCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new VideoModeCommand(new AtemState
        {
            Settings =
            {
                VideoMode = testCase.Command.VideoMode
            }
        });
    }
}
