using AtemSharp.State.Audio.ClassicAudio;
using AudioMixerInputCommand = AtemSharp.Commands.Audio.ClassicAudio.AudioMixerInputCommand;

namespace AtemSharp.Tests.Commands.Audio;

[TestFixture]
public class AudioMixerInputCommandTests : SerializedCommandTestBase<AudioMixerInputCommand,
    AudioMixerInputCommandTests.CommandData>
{
    protected override Range[] GetFloatingPointByteRanges()
    {
        return
        [
            6..8, // bytes 6-7 for Gain
            8..10 // bytes 8-9 for Balance
        ];
    }

    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
        public AudioMixOption MixOption { get; set; }
        public double Gain { get; set; }
        public double Balance { get; set; }
        public bool RcaToXlrEnabled { get; set; }
    }

    protected override AudioMixerInputCommand CreateSut(TestCaseData testCase)
    {
        return new AudioMixerInputCommand(new ClassicAudioChannel
        {
            Id = testCase.Command.Index,
            MixOption = testCase.Command.MixOption,
            Gain = testCase.Command.Gain,
            Balance = testCase.Command.Balance,
            RcaToXlrEnabled = testCase.Command.RcaToXlrEnabled
        });
    }

    [Test]
    public void GainProperty_WithValidValues_ShouldSetCorrectly()
    {
        // Arrange
        var command = new AudioMixerInputCommand(new ClassicAudioChannel());

        // Act
        command.Gain = 1.0;

        // Assert
        Assert.That(command.Flag & (1 << 1), Is.Not.EqualTo(0), "Gain flag should be set");
    }


    [Test]
    public void BalanceProperty_WithValidValues_ShouldSetCorrectly()
    {
        // Arrange
        var command = new AudioMixerInputCommand(new ClassicAudioChannel());

        // Act
        command.Balance = 1.0;

        // Assert
        Assert.That(command.Flag & (1 << 2), Is.Not.EqualTo(0), "Balance flag should be set");
    }

    [Test]
    public void MixOptionProperty_WithValidValues_ShouldSetCorrectlyAndSetFlag()
    {
        // Arrange
        var command = new AudioMixerInputCommand(new ClassicAudioChannel());

        // Act
        command.MixOption = AudioMixOption.AudioFollowVideo;

        // Assert
        Assert.That(command.Flag & (1 << 0), Is.Not.EqualTo(0), "MixOption flag should be set");
    }

    [Test]
    public void RcaToXlrEnabledProperty_ShouldSetCorrectlyAndSetFlag()
    {
        // Arrange
        var command = new AudioMixerInputCommand(new ClassicAudioChannel());

        // Act
        command.RcaToXlrEnabled = true;

        // Assert
        Assert.That(command.Flag & (1 << 3), Is.Not.EqualTo(0), "RcaToXlrEnabled flag should be set");
    }

    [Test]
    public void Constructor_ShouldInitializeFromStateWithoutSettingFlags()
    {
        // Arrange
        var command = new AudioMixerInputCommand(new ClassicAudioChannel());

        // Assert
        Assert.That(command.Flag, Is.EqualTo(0), "No flags should be set when initializing from state");
    }

    [Test]
    public void SetMultipleProperties_ShouldSetCombinedFlags()
    {
        // Arrange
        var command = new AudioMixerInputCommand(new ClassicAudioChannel());

        // Act
        command.MixOption = AudioMixOption.On; // Flag 0x01
        command.Gain = -10.0; // Flag 0x02
        command.RcaToXlrEnabled = true; // Flag 0x08

        // Assert
        Assert.That(command.Flag, Is.EqualTo(0x01 | 0x02 | 0x08));
    }
}
