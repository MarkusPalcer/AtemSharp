using AtemSharp.Commands.Audio;
using AtemSharp.State.Audio.ClassicAudio;

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
    [TestCase(-60.0)]
    [TestCase(-30.0)]
    [TestCase(0.0)]
    [TestCase(3.5)]
    [TestCase(6.0)]
    public void GainProperty_WithValidValues_ShouldSetCorrectly(double validGain)
    {
        // Arrange
        var command = new AudioMixerInputCommand(new ClassicAudioChannel());

        // Act
        command.Gain = validGain;

        // Assert
        Assert.That(command.Gain, Is.EqualTo(validGain));
        Assert.That(command.Flag & (1 << 1), Is.Not.EqualTo(0), "Gain flag should be set");
    }


    [TestCase(-50.0)]
    [TestCase(-25.0)]
    [TestCase(0.0)]
    [TestCase(25.0)]
    [TestCase(50.0)]
    public void BalanceProperty_WithValidValues_ShouldSetCorrectly(double validBalance)
    {
        // Arrange
        var command = new AudioMixerInputCommand(new ClassicAudioChannel());

        // Act
        command.Balance = validBalance;

        // Assert
        Assert.That(command.Balance, Is.EqualTo(validBalance));
        Assert.That(command.Flag & (1 << 2), Is.Not.EqualTo(0), "Balance flag should be set");
    }

    [TestCase(AudioMixOption.Off)]
    [TestCase(AudioMixOption.On)]
    [TestCase(AudioMixOption.AudioFollowVideo)]
    public void MixOptionProperty_WithValidValues_ShouldSetCorrectlyAndSetFlag(AudioMixOption mixOption)
    {
        // Arrange
        var command = new AudioMixerInputCommand(new ClassicAudioChannel());

        // Act
        command.MixOption = mixOption;

        // Assert
        Assert.That(command.MixOption, Is.EqualTo(mixOption));
        Assert.That(command.Flag & (1 << 0), Is.Not.EqualTo(0), "MixOption flag should be set");
    }

    [TestCase(true)]
    [TestCase(false)]
    public void RcaToXlrEnabledProperty_ShouldSetCorrectlyAndSetFlag(bool enabled)
    {
        // Arrange
        var command = new AudioMixerInputCommand(new ClassicAudioChannel());

        // Act
        command.RcaToXlrEnabled = enabled;

        // Assert
        Assert.That(command.RcaToXlrEnabled, Is.EqualTo(enabled));
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
