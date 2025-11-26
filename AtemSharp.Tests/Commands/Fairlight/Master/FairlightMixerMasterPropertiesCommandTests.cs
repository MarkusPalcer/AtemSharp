using AtemSharp.State.Audio.Fairlight;
using FairlightMixerMasterPropertiesCommand = AtemSharp.Commands.Audio.Fairlight.Master.FairlightMixerMasterPropertiesCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Master;

public class FairlightMixerMasterPropertiesCommandTests
    : SerializedCommandTestBase<FairlightMixerMasterPropertiesCommand, FairlightMixerMasterPropertiesCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool AudioFollowVideoCrossfadeTransitionEnabled { get; set; }
    }


    protected override FairlightMixerMasterPropertiesCommand CreateSut(TestCaseData testCase)
    {
        return new FairlightMixerMasterPropertiesCommand(new MasterProperties
        {
            AudioFollowsVideo = testCase.Command.AudioFollowVideoCrossfadeTransitionEnabled
        });
    }
}
