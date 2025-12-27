using AtemSharp.Commands.DeviceProfile;
using AtemSharp.State;
using AtemSharp.State.Info;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Commands.DeviceProfile;

[TestFixture]
internal class AudioMixerConfigCommandTests : DeserializedCommandTestBase<AudioMixerConfigCommand, AudioMixerConfigCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Inputs { get; set; }
        public byte Monitors { get; set; }
        public byte Headphones { get; set; }
    }

    internal override void CompareCommandProperties(AudioMixerConfigCommand actualCommand, CommandData expectedData)
    {
        Assert.That(actualCommand.Inputs, Is.EqualTo(expectedData.Inputs));
        Assert.That(actualCommand.Monitors, Is.EqualTo(expectedData.Monitors));
        Assert.That(actualCommand.Headphones, Is.EqualTo(expectedData.Headphones));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.Info.Mixer.As<ClassicAudioMixerInfo>().Inputs, Is.EqualTo(expectedData.Inputs));
        Assert.That(state.Info.Mixer.As<ClassicAudioMixerInfo>().Monitors, Is.EqualTo(expectedData.Monitors));
        Assert.That(state.Info.Mixer.As<ClassicAudioMixerInfo>().Headphones, Is.EqualTo(expectedData.Headphones));
    }
}
