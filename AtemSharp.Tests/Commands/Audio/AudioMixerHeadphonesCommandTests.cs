using AtemSharp.State.Audio.ClassicAudio;
using AudioMixerHeadphonesCommand = AtemSharp.Commands.Audio.ClassicAudio.AudioMixerHeadphonesCommand;

namespace AtemSharp.Tests.Commands.Audio;

[TestFixture]
public class AudioMixerHeadphonesCommandTests : SerializedCommandTestBase<AudioMixerHeadphonesCommand,
    AudioMixerHeadphonesCommandTests.CommandData>
{
    protected override Range[] GetFloatingPointByteRanges()
    {
        return
        [
            2..4, // bytes 2-3 for Gain
            4..6, // bytes 4-5 for ProgramOutGain
            6..8, // bytes 6-7 for TalkbackGain
            8..10 // bytes 8-9 for SidetoneGain
        ];
    }

    public class CommandData : CommandDataBase
    {
        public double Gain { get; set; }
        public double ProgramOutGain { get; set; }
        public double TalkbackGain { get; set; }
        public double SidetoneGain { get; set; }
    }

    protected override AudioMixerHeadphonesCommand CreateSut(TestCaseData testCase)
    {
        // Create command with valid state
        return new AudioMixerHeadphonesCommand(new ClassicAudioState
        {
            Headphones =
            {
                Gain = testCase.Command.Gain,
                ProgramOutGain = testCase.Command.ProgramOutGain,
                TalkbackGain = testCase.Command.TalkbackGain,
                SidetoneGain = testCase.Command.SidetoneGain
            }
        });
    }

    [Test]
    public void GainProperty_WithValidValues_ShouldSetCorrectlyAndSetFlag()
    {
        // Arrange
        var command = new AudioMixerHeadphonesCommand(new ClassicAudioState());

        // Act
        command.Gain = 3.5;

        // Assert
        Assert.That(command.Gain, Is.EqualTo(3.5));
        Assert.That(command.Flag & (1 << 0), Is.Not.EqualTo(0), "Gain flag should be set");
    }

    [Test]
    public void ProgramOutGainProperty_WithValidValues_ShouldSetCorrectlyAndSetFlag()
    {
        // Arrange
        var command = new AudioMixerHeadphonesCommand(new ClassicAudioState());

        // Act
        command.ProgramOutGain = 2.5;

        // Assert
        Assert.That(command.ProgramOutGain, Is.EqualTo(2.5));
        Assert.That(command.Flag & (1 << 1), Is.Not.EqualTo(0), "ProgramOutGain flag should be set");
    }

    [Test]
    public void TalkbackGainProperty_WithValidValues_ShouldSetCorrectlyAndSetFlag()
    {
        // Arrange
        var command = new AudioMixerHeadphonesCommand(new ClassicAudioState());

        // Act
        command.TalkbackGain = 2.5;

        // Assert
        Assert.That(command.TalkbackGain, Is.EqualTo(2.5));
        Assert.That(command.Flag & (1 << 2), Is.Not.EqualTo(0), "TalkbackGain flag should be set");
    }

    [Test]
    public void SidetoneGainProperty_WithValidValues_ShouldSetCorrectlyAndSetFlag()
    {
        // Arrange
        var command = new AudioMixerHeadphonesCommand(new ClassicAudioState());

        // Act
        command.SidetoneGain = 2.5;

        // Assert
        Assert.That(command.SidetoneGain, Is.EqualTo(2.5));
        Assert.That(command.Flag & (1 << 3), Is.Not.EqualTo(0), "SidetoneGain flag should be set");
    }

    [Test]
    public void SetMultipleProperties_ShouldSetCombinedFlags()
    {
        // Arrange
        var command = new AudioMixerHeadphonesCommand(new ClassicAudioState());

        // Act
        command.Gain = -10.0; // Flag 0x01
        command.TalkbackGain = -15.0; // Flag 0x04
        command.SidetoneGain = -5.0; // Flag 0x08

        // Assert
        Assert.That(command.Flag, Is.EqualTo(0x01 | 0x04 | 0x08));
    }

    [Test]
    public void SetAllProperties_ShouldSetAllFlags()
    {
        // Arrange
        var command = new AudioMixerHeadphonesCommand(new ClassicAudioState());

        // Act
        command.Gain = -5.0; // Flag 0x01
        command.ProgramOutGain = -8.0; // Flag 0x02
        command.TalkbackGain = -12.0; // Flag 0x04
        command.SidetoneGain = -2.0; // Flag 0x08

        // Assert
        Assert.That(command.Flag, Is.EqualTo(0x0F), "All flags should be set");
    }

    [Test]
    public void Properties_WhenSetMultipleTimes_ShouldMaintainFlags()
    {
        // Arrange
        var command = new AudioMixerHeadphonesCommand(new ClassicAudioState());

        // Act
        command.Gain = -5.0; // Set flag first time
        command.Gain = -10.0; // Setting again should keep flag
        command.ProgramOutGain = -3.0; // Set different flag

        // Assert
        Assert.That(command.Gain, Is.EqualTo(-10.0));
        Assert.That(command.ProgramOutGain, Is.EqualTo(-3.0));
        Assert.That(command.Flag & (1 << 0), Is.Not.EqualTo(0), "Gain flag should remain set");
        Assert.That(command.Flag & (1 << 1), Is.Not.EqualTo(0), "ProgramOutGain flag should be set");
        Assert.That(command.Flag, Is.EqualTo(0x03), "Both flags should be set");
    }
}
