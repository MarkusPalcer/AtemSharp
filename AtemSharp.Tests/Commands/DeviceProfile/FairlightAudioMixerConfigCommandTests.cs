using AtemSharp.Commands.DeviceProfile;
using AtemSharp.State;
using AtemSharp.State.Info;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Commands.DeviceProfile;

[TestFixture]
internal class FairlightAudioMixerConfigCommandTests : DeserializedCommandTestBase<FairlightAudioMixerConfigCommand,
    FairlightAudioMixerConfigCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Inputs { get; set; }
        public byte Monitors { get; set; }
    }

    internal override void CompareCommandProperties(FairlightAudioMixerConfigCommand actualCommand, CommandData expectedData, TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        Assert.That(actualCommand.Inputs, Is.EqualTo(expectedData.Inputs));
        Assert.That(actualCommand.Monitors, Is.EqualTo(expectedData.Monitors));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.Info.Mixer.As<FairlightAudioMixerInfo>().Inputs, Is.EqualTo(expectedData.Inputs));
        Assert.That(state.Info.Mixer.As<FairlightAudioMixerInfo>().Monitors, Is.EqualTo(expectedData.Monitors));
    }
}
