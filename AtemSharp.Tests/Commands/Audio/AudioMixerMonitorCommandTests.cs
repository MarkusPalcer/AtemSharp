using AtemSharp.State;
using AtemSharp.State.Audio.ClassicAudio;
using AudioMixerMonitorCommand = AtemSharp.Commands.Audio.AudioMixerMonitorCommand;

namespace AtemSharp.Tests.Commands.Audio;

[TestFixture]
public class AudioMixerMonitorCommandTests : SerializedCommandTestBase<AudioMixerMonitorCommand,
    AudioMixerMonitorCommandTests.CommandData>
{
    protected override Range[] GetFloatingPointByteRanges()
    {
        return
        [
            2..4, // bytes 2-3 for Gain
            10..12 // bytes 10-11 for DimLevel
        ];
    }

    public class CommandData : CommandDataBase
    {
        public bool Enabled { get; set; }
        public double Gain { get; set; }
        public bool Mute { get; set; }
        public bool Solo { get; set; }
        public ushort SoloSource { get; set; }
        public bool Dim { get; set; }
        public double DimLevel { get; set; }
    }

    protected override AudioMixerMonitorCommand CreateSut(TestCaseData testCase)
    {
        // Create command with valid state
        var command = new AudioMixerMonitorCommand(CreateStateWithMonitor());

        // Set the actual values that should be written (like TypeScript this.properties)
        command.Enabled = testCase.Command.Enabled;
        command.Gain = testCase.Command.Gain;
        command.Mute = testCase.Command.Mute;
        command.Solo = testCase.Command.Solo;
        command.SoloSource = testCase.Command.SoloSource;
        command.Dim = testCase.Command.Dim;
        command.DimLevel = testCase.Command.DimLevel;

        return command;
    }

    /// <summary>
    /// Creates an AtemState with valid monitor configuration
    /// </summary>
    private static AtemState CreateStateWithMonitor()
    {
        var state = new AtemState
        {
            Audio = new ClassicAudioState
            {
                Monitor =
                {
                    Enabled = false,
                    Gain = 0.0,
                    Mute = false,
                    Solo = false,
                    SoloSource = 0,
                    Dim = false,
                    DimLevel = 0.0
                }
            }
        };
        return state;
    }

    [Test]
    public void SetEnabled_ShouldSetCorrectFlag()
    {
        // Arrange
        var command = new AudioMixerMonitorCommand(CreateStateWithMonitor());

        // Act
        command.Enabled = true;

        // Assert
        Assert.That(command.Enabled, Is.EqualTo(true));
        Assert.That(command.Flag, Is.EqualTo(0x01), "Flag should be set for Enabled");
    }

    [Test]
    public void SetGain_ShouldSetCorrectFlag()
    {
        // Arrange
        var command = new AudioMixerMonitorCommand(CreateStateWithMonitor());

        // Act
        command.Gain = -10.5;

        // Assert
        Assert.That(command.Gain, Is.EqualTo(-10.5));
        Assert.That(command.Flag, Is.EqualTo(0x02), "Flag should be set for Gain");
    }

    [Test]
    public void SetMute_ShouldSetCorrectFlag()
    {
        // Arrange
        var command = new AudioMixerMonitorCommand(CreateStateWithMonitor());

        // Act
        command.Mute = true;

        // Assert
        Assert.That(command.Mute, Is.EqualTo(true));
        Assert.That(command.Flag, Is.EqualTo(0x04), "Flag should be set for Mute");
    }

    [Test]
    public void SetSolo_ShouldSetCorrectFlag()
    {
        // Arrange
        var command = new AudioMixerMonitorCommand(CreateStateWithMonitor());

        // Act
        command.Solo = true;

        // Assert
        Assert.That(command.Solo, Is.EqualTo(true));
        Assert.That(command.Flag, Is.EqualTo(0x08), "Flag should be set for Solo");
    }

    [Test]
    public void SetSoloSource_ShouldSetCorrectFlag()
    {
        // Arrange
        var command = new AudioMixerMonitorCommand(CreateStateWithMonitor());

        // Act
        command.SoloSource = 1001;

        // Assert
        Assert.That(command.SoloSource, Is.EqualTo(1001));
        Assert.That(command.Flag, Is.EqualTo(0x10), "Flag should be set for SoloSource");
    }

    [Test]
    public void SetDim_ShouldSetCorrectFlag()
    {
        // Arrange
        var command = new AudioMixerMonitorCommand(CreateStateWithMonitor());

        // Act
        command.Dim = true;

        // Assert
        Assert.That(command.Dim, Is.EqualTo(true));
        Assert.That(command.Flag, Is.EqualTo(0x20), "Flag should be set for Dim");
    }

    [Test]
    public void SetDimLevel_ShouldSetCorrectFlag()
    {
        // Arrange
        var command = new AudioMixerMonitorCommand(CreateStateWithMonitor());

        // Act
        command.DimLevel = 0.5; // 50%

        // Assert
        Assert.That(command.DimLevel, Is.EqualTo(0.5));
        Assert.That(command.Flag, Is.EqualTo(0x40), "Flag should be set for DimLevel");
    }

    [Test]
    public void SetMultipleProperties_ShouldSetCorrectFlags()
    {
        // Arrange
        var command = new AudioMixerMonitorCommand(CreateStateWithMonitor());

        // Act
        command.Enabled = true; // 0x01
        command.Gain = -5.0; // 0x02
        command.Mute = true; // 0x04

        // Assert
        Assert.That(command.Flag, Is.EqualTo(0x07), "Multiple flags should be set (0x01 | 0x02 | 0x04)");
    }
}
