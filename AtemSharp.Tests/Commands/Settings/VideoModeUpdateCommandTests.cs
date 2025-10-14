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
public class VideoModeUpdateCommandTests
{
    [Test]
    public void Deserialize_ShouldReadVideoModeFromFirstByte()
    {
        // Arrange
        var testData = new byte[] { 13, 0, 0, 0 }; // VideoMode.N1080p5994 = 13
        using var stream = new MemoryStream(testData);

        // Act
        var command = VideoModeUpdateCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.Mode, Is.EqualTo(VideoMode.N1080p5994), "Should deserialize video mode correctly");
    }

    [Test]
    public void Deserialize_ShouldHandleDifferentVideoModes()
    {
        var testCases = new[]
        {
            (new byte[] { 0, 0, 0, 0 }, VideoMode.N525i5994NTSC),
            (new byte[] { 1, 0, 0, 0 }, VideoMode.P625i50PAL),
            (new byte[] { 4, 0, 0, 0 }, VideoMode.P720p50),
            (new byte[] { 13, 0, 0, 0 }, VideoMode.N1080p5994),
            (new byte[] { 25, 0, 0, 0 }, VideoMode.N8KHDp5994)
        };

        foreach (var (data, expectedMode) in testCases)
        {
            // Arrange
            using var stream = new MemoryStream(data);

            // Act
            var command = VideoModeUpdateCommand.Deserialize(stream, ProtocolVersion.V7_2);

            // Assert
            Assert.That(command.Mode, Is.EqualTo(expectedMode),
                       $"Byte value {data[0]} should deserialize to {expectedMode}");
        }
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
}
