using AtemSharp.Commands.Settings;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Settings;

/// <summary>
/// Manual tests for VideoModeCommand since test data from libatem-data.json
/// contains inconsistent expected values for the mode property.
/// All test cases have Mode=N525i5994NTSC but expect different byte outputs.
/// </summary>
[TestFixture]
public class VideoModeCommandTests
{
    [Test]
    public void Serialize_ShouldWriteVideoModeValue()
    {
        // Arrange
        var state = new AtemState
        {
            Settings = new SettingsState
            {
                VideoMode = VideoMode.N1080p5994
            }
        };
        
        var command = new VideoModeCommand(state);
        command.Mode = VideoMode.N1080p5994; // 13 = 0x0D
        
        // Act
        var result = command.Serialize(ProtocolVersion.V7_2);
        
        // Assert
        Assert.That(result.Length, Is.EqualTo(4), "Serialized command should be 4 bytes");
        Assert.That(result[0], Is.EqualTo(13), "First byte should be the video mode value");
        Assert.That(result[1], Is.EqualTo(0), "Second byte should be padding");
        Assert.That(result[2], Is.EqualTo(0), "Third byte should be padding");
        Assert.That(result[3], Is.EqualTo(0), "Fourth byte should be padding");
    }
    
    [Test]
    public void Serialize_ShouldWriteDifferentVideoModes()
    {
        // Test various video modes to ensure enum values are written correctly
        var testCases = new[]
        {
            (VideoMode.N525i5994NTSC, (byte)0),
            (VideoMode.P625i50PAL, (byte)1),
            (VideoMode.P720p50, (byte)4),
            (VideoMode.N1080p5994, (byte)13),
            (VideoMode.N8KHDp5994, (byte)25)
        };
        
        foreach (var (mode, expectedByte) in testCases)
        {
            // Arrange
            var state = new AtemState
            {
                Settings = new SettingsState { VideoMode = mode }
            };
            
            var command = new VideoModeCommand(state);
            command.Mode = mode;
            
            // Act
            var result = command.Serialize(ProtocolVersion.V7_2);
            
            // Assert
            Assert.That(result[0], Is.EqualTo(expectedByte), 
                       $"Video mode {mode} should serialize to byte value {expectedByte}");
        }
    }
}