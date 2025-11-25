using AtemSharp.Commands.DeviceProfile;
using AtemSharp.Enums;
using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Tests.Commands.DeviceProfile;

[TestFixture]
// No inheritance from base class as test data does not contain test cases for this command
public class VideoMixerConfigCommandTests
{

    [Test]
    public void Deserialize_WithBasicData_ShouldCreateCorrectCommand()
    {
        // Arrange - Create a mock data packet
        var data = new List<byte>();

        // Write count (2 modes) - Big Endian
        var count = BitConverter.GetBytes((ushort)2);
        if (BitConverter.IsLittleEndian) Array.Reverse(count);
        data.AddRange(count);

        var padding = BitConverter.GetBytes((ushort)0);
        if (BitConverter.IsLittleEndian) Array.Reverse(padding);
        data.AddRange(padding);

        // Mode 1: 1080p25 with multiviewer modes and no reconfig
        data.Add((byte)VideoMode.P1080p25);    // mode
        data.AddRange(new byte[3]);            // padding

        var multiviewer1 = BitConverter.GetBytes(0b00010100u);
        if (BitConverter.IsLittleEndian) Array.Reverse(multiviewer1);
        data.AddRange(multiviewer1);   // multiviewer modes (bits 2 and 4 set)

        var downconvert1 = BitConverter.GetBytes(0b00000100u);
        if (BitConverter.IsLittleEndian) Array.Reverse(downconvert1);
        data.AddRange(downconvert1);   // downconvert modes (bit 2 set)

        // Mode 2: 4K with different settings and no reconfig (protocol < V8_0)
        data.Add((byte)VideoMode.N4KHDp2398);  // mode
        data.AddRange(new byte[3]);            // padding

        var multiviewer2 = BitConverter.GetBytes(0b00000001u);
        if (BitConverter.IsLittleEndian) Array.Reverse(multiviewer2);
        data.AddRange(multiviewer2);   // multiviewer modes (bit 0 set)

        var downconvert2 = BitConverter.GetBytes(0b00001010u);
        if (BitConverter.IsLittleEndian) Array.Reverse(downconvert2);
        data.AddRange(downconvert2);   // downconvert modes (bits 1 and 3 set)

        // Act
        var command = VideoMixerConfigCommand.Deserialize(data.ToArray().AsSpan(), ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.SupportedVideoModes, Is.Not.Null);
        Assert.That(command.SupportedVideoModes.Length, Is.EqualTo(2));

        // Check first mode
        var mode1 = command.SupportedVideoModes[0];
        Assert.That(mode1.Mode, Is.EqualTo(VideoMode.P1080p25));
        Assert.That(mode1.RequiresReconfiguration, Is.False);
        Assert.That(mode1.MultiviewerModes, Is.EqualTo(new[] { VideoMode.N525i5994169, VideoMode.P720p50 }));
        Assert.That(mode1.DownConvertModes, Is.EqualTo(new[] { VideoMode.N525i5994169 }));

        // Check second mode
        var mode2 = command.SupportedVideoModes[1];
        Assert.That(mode2.Mode, Is.EqualTo(VideoMode.N4KHDp2398));
        Assert.That(mode2.RequiresReconfiguration, Is.False);
        Assert.That(mode2.MultiviewerModes, Is.EqualTo(new[] { VideoMode.N525i5994NTSC }));
        Assert.That(mode2.DownConvertModes, Is.EqualTo(new[] { VideoMode.P625i50PAL, VideoMode.P625i50169 }));
    }

    [Test]
    public void Deserialize_WithV8Protocol_ShouldIncludeRequiresReconfig()
    {
        // Arrange - Create a mock data packet with requires reconfig flag
        var data = new List<byte>();

        // Write count (1 mode) - Big Endian
        var count = BitConverter.GetBytes((ushort)1);
        if (BitConverter.IsLittleEndian) Array.Reverse(count);
        data.AddRange(count);

        var padding = BitConverter.GetBytes((ushort)0);
        if (BitConverter.IsLittleEndian) Array.Reverse(padding);
        data.AddRange(padding);

        // Mode 1: with requires reconfig flag
        data.Add((byte)VideoMode.P1080p25);    // mode
        data.AddRange(new byte[3]);            // padding

        var multiviewer = BitConverter.GetBytes(0u);
        if (BitConverter.IsLittleEndian) Array.Reverse(multiviewer);
        data.AddRange(multiviewer);       // multiviewer modes

        var downconvert = BitConverter.GetBytes(0u);
        if (BitConverter.IsLittleEndian) Array.Reverse(downconvert);
        data.AddRange(downconvert);       // downconvert modes

        data.Add(1);                           // requires reconfig = true

        // Act
        var command = VideoMixerConfigCommand.Deserialize(data.ToArray().AsSpan(), ProtocolVersion.V8_0);

        // Assert
        Assert.That(command.SupportedVideoModes.Length, Is.EqualTo(1));
        Assert.That(command.SupportedVideoModes[0].RequiresReconfiguration, Is.True);
    }

    [Test]
    public void ApplyToState_ShouldUpdateSupportedVideoModes()
    {
        // Arrange
        var state = new AtemState();
        var supportedModes = new[]
        {
            new SupportedVideoMode
            {
                Mode = VideoMode.P1080p25,
                RequiresReconfiguration = false,
                MultiviewerModes = [VideoMode.P720p50, VideoMode.P1080p25],
                DownConvertModes = [VideoMode.P720p50]
            },
            new SupportedVideoMode
            {
                Mode = VideoMode.N4KHDp2398,
                RequiresReconfiguration = true,
                MultiviewerModes = [VideoMode.P1080p25],
                DownConvertModes = [VideoMode.P1080p25, VideoMode.P720p50]
            }
        };

        var command = new VideoMixerConfigCommand
        {
            SupportedVideoModes = supportedModes
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Info.SupportedVideoModes, Is.Not.Null);
        Assert.That(state.Info.SupportedVideoModes.Length, Is.EqualTo(2));
        Assert.That(state.Info.SupportedVideoModes, Is.SameAs(supportedModes));
    }

    [Test]
    public void ApplyToState_WithNullState_ShouldThrowException()
    {
        // Arrange
        var command = new VideoMixerConfigCommand
        {
            SupportedVideoModes = []
        };

        // Act & Assert
        Assert.Throws<NullReferenceException>(() => command.ApplyToState(null!));
    }

    [Test]
    public void ApplyToState_WithEmptyArray_ShouldSetEmptyArray()
    {
        // Arrange
        var state = new AtemState();
        var command = new VideoMixerConfigCommand
        {
            SupportedVideoModes = []
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Info.SupportedVideoModes, Is.Not.Null);
        Assert.That(state.Info.SupportedVideoModes.Length, Is.EqualTo(0));
    }
}
