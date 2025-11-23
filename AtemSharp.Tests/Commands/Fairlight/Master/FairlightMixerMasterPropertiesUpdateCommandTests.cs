using FairlightMixerMasterPropertiesUpdateCommand = AtemSharp.Commands.Fairlight.Master.FairlightMixerMasterPropertiesUpdateCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Master;

public class FairlightMixerMasterPropertiesUpdateCommandTests : DeserializedCommandTestBase<FairlightMixerMasterPropertiesUpdateCommand, FairlightMixerMasterPropertiesUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool AudioFollowVideoCrossfadeTransitionEnabled { get; set; }
    }

    protected override void CompareCommandProperties(FairlightMixerMasterPropertiesUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.AudioFollowsVideo, Is.EqualTo(expectedData.AudioFollowVideoCrossfadeTransitionEnabled));
    }
}
