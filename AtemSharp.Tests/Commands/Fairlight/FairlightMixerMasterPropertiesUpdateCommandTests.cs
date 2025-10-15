using AtemSharp.Commands.Fairlight;

namespace AtemSharp.Tests.Commands.Fairlight;

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
