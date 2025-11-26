using AtemSharp.State.Audio.ClassicAudio;
using AudioMixerResetPeaksCommand = AtemSharp.Commands.Audio.ClassicAudio.AudioMixerResetPeaksCommand;

namespace AtemSharp.Tests.Commands.Audio;

[TestFixture]
public class AudioMixerResetPeaksCommandTests : SerializedCommandTestBase<AudioMixerResetPeaksCommand,
    AudioMixerResetPeaksCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool All { get; set; }
        public ushort Input { get; set; }
        public bool Master { get; set; }
        public bool Monitor { get; set; }
    }

    protected override AudioMixerResetPeaksCommand CreateSut(TestCaseData testCase)
    {
        return new AudioMixerResetPeaksCommand(new ClassicAudioChannel
        {
            Id = testCase.Command.Input
        })
        {
            All = testCase.Command.All,
            Master = testCase.Command.Master,
            Monitor = testCase.Command.Monitor,
        };
    }

    [Test]
    public void SetAll_ShouldSetCorrectFlag()
    {
        // Arrange
        var command = new AudioMixerResetPeaksCommand(new ClassicAudioChannel());

        // Act
        command.All = true;

        // Assert
        Assert.That(command.Flag, Is.EqualTo(1 << 0)); // Bit 0 should be set
    }

    [Test]
    public void SetMaster_ShouldSetCorrectFlag()
    {
        // Arrange
        var command = new AudioMixerResetPeaksCommand(new ClassicAudioChannel());

        // Act
        command.Master = true;

        // Assert
        Assert.That(command.Flag, Is.EqualTo(1 << 2)); // Bit 2 should be set
    }

    [Test]
    public void SetMonitor_ShouldSetCorrectFlag()
    {
        // Arrange
        var command = new AudioMixerResetPeaksCommand(new ClassicAudioChannel());

        // Act
        command.Monitor = true;

        // Assert
        Assert.That(command.Flag, Is.EqualTo(1 << 3)); // Bit 3 should be set
    }

    [Test]
    public void SetMultipleProperties_ShouldSetCombinedFlags()
    {
        // Arrange
        var command = new AudioMixerResetPeaksCommand(new ClassicAudioChannel());

        // Act
        command.All = true; // Bit 0
        command.Master = true; // Bit 2

        // Assert
        Assert.That(command.Flag, Is.EqualTo((1 << 0) | (1 << 2))); // Bits 0 and 2 should be set
    }
}
