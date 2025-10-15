using AtemSharp.Commands.Fairlight;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Fairlight;

public class FairlightMixerMasterPropertiesCommandTests : SerializedCommandTestBase<FairlightMixerMasterPropertiesCommand, FairlightMixerMasterPropertiesCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
public bool AudioFollowVideoCrossfadeTransitionEnabled { get; set; }
    }


    protected override FairlightMixerMasterPropertiesCommand CreateSut(TestCaseData testCase)
    {
        var state = new AtemState
        {
            Audio = new FairlightAudioState()
        };

        return new FairlightMixerMasterPropertiesCommand(state)
        {
            AudioFollowsVideo = testCase.Command.AudioFollowVideoCrossfadeTransitionEnabled
        };
    }
}
