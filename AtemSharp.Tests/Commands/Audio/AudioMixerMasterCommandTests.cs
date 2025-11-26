using AtemSharp.State.Audio.ClassicAudio;
using AudioMixerMasterCommand = AtemSharp.Commands.Audio.ClassicAudio.AudioMixerMasterCommand;

namespace AtemSharp.Tests.Commands.Audio;

[TestFixture]
public class AudioMixerMasterCommandTests : SerializedCommandTestBase<AudioMixerMasterCommand,
    AudioMixerMasterCommandTests.CommandData>
{
    protected override Range[] GetFloatingPointByteRanges()
    {
        return
        [
            2..4, // bytes 2-3 for Gain
            4..6 // bytes 4-5 for Balance
        ];
    }

    public class CommandData : CommandDataBase
    {
        public double Gain { get; set; }
        public double Balance { get; set; }
        public bool FollowFadeToBlack { get; set; }
    }

    protected override AudioMixerMasterCommand CreateSut(TestCaseData testCase)
    {
        return new AudioMixerMasterCommand(new ClassicAudioState
        {
            Master =
            {
                Gain = testCase.Command.Gain,
                Balance = testCase.Command.Balance,
                FollowFadeToBlack = testCase.Command.FollowFadeToBlack
            }
        });
    }

    [Test]
    public void SetGain_WithValidValue_ShouldSetFlag()
    {
        // Arrange
        var command = new AudioMixerMasterCommand(new ClassicAudioState());

        // Act
        command.Gain = -12.5;

        // Assert
        Assert.That(command.Flag & (1 << 0), Is.Not.EqualTo(0), "Gain flag should be set");
    }

    [Test]
    public void SetBalance_WithValidValue_ShouldSetPropertyAndFlag()
    {
        // Arrange
        var command = new AudioMixerMasterCommand(new ClassicAudioState());

        // Act
        command.Balance = 25.0;

        // Assert
        Assert.That(command.Flag & (1 << 1), Is.Not.EqualTo(0), "Balance flag should be set");
    }


    [Test]
    public void SetFollowFadeToBlack_WithValue_ShouldSetFlag()
    {
        // Arrange
        var command = new AudioMixerMasterCommand(new ClassicAudioState());

        // Act
        command.FollowFadeToBlack = true;

        // Assert
        Assert.That(command.Flag & (1 << 2), Is.Not.EqualTo(0), "FollowFadeToBlack flag should be set");
    }

    [Test]
    public void SetMultipleProperties_ShouldSetMultipleFlags()
    {
        // Arrange
        var state = new ClassicAudioState
        {
            Master =
            {
                Gain = 0.0,
                Balance = 0.0,
                FollowFadeToBlack = false
            }
        };
        var command = new AudioMixerMasterCommand(state);

        // Act
        command.Gain = -5.0;
        command.Balance = 10.0;
        command.FollowFadeToBlack = true;

        // Assert
        Assert.That(command.Flag, Is.EqualTo(0x07), "All three flags should be set");
    }
}
