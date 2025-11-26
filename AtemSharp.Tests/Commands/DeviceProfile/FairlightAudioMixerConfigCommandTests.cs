using AtemSharp.Commands.DeviceProfile;

namespace AtemSharp.Tests.Commands.DeviceProfile;

[TestFixture]
public class FairlightAudioMixerConfigCommandTests : DeserializedCommandTestBase<FairlightAudioMixerConfigCommand,
    FairlightAudioMixerConfigCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Inputs { get; set; }
        public byte Monitors { get; set; }
    }

    protected override void CompareCommandProperties(FairlightAudioMixerConfigCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.Inputs, Is.EqualTo(expectedData.Inputs));
        Assert.That(actualCommand.Monitors, Is.EqualTo(expectedData.Monitors));
    }
}
