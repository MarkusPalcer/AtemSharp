using AtemSharp.Helpers;
using AtemSharp.State;

namespace AtemSharp.Commands.Fairlight.Master;

[Command("FMPP")]
public partial class FairlightMixerMasterPropertiesUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private bool _audioFollowsVideo;

    public void ApplyToState(AtemState state)
    {
        state.GetFairlight().Master.AudioFollowsVideo = AudioFollowsVideo;
    }
}
