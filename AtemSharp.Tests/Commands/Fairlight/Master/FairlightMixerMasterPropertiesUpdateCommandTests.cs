using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;
using FairlightMixerMasterPropertiesUpdateCommand = AtemSharp.Commands.Audio.Fairlight.Master.FairlightMixerMasterPropertiesUpdateCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Master;

public class FairlightMixerMasterPropertiesUpdateCommandTests : DeserializedCommandTestBase<FairlightMixerMasterPropertiesUpdateCommand,
    FairlightMixerMasterPropertiesUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool AudioFollowVideoCrossfadeTransitionEnabled { get; set; }
    }

    protected override void CompareCommandProperties(FairlightMixerMasterPropertiesUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.AudioFollowsVideo, Is.EqualTo(expectedData.AudioFollowVideoCrossfadeTransitionEnabled));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Audio = new FairlightAudioState();
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.GetFairlight().Master.AudioFollowsVideo, Is.EqualTo(expectedData.AudioFollowVideoCrossfadeTransitionEnabled));
    }
}
