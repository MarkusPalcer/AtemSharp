using AtemSharp.Commands.DeviceProfile;

namespace AtemSharp.Tests.Commands.DeviceProfile;

[TestFixture]
public class AudioMixerConfigCommandTests : DeserializedCommandTestBase<AudioMixerConfigCommand, AudioMixerConfigCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Inputs { get; set; }
        public byte Monitors { get; set; }
        public byte Headphones { get; set; }
    }

    protected override void CompareCommandProperties(AudioMixerConfigCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.Inputs, Is.EqualTo(expectedData.Inputs));
        Assert.That(actualCommand.Monitors, Is.EqualTo(expectedData.Monitors));
        Assert.That(actualCommand.Headphones, Is.EqualTo(expectedData.Headphones));
    }
}
