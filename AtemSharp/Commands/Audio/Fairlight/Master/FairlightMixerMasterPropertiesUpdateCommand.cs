using AtemSharp.State;

namespace AtemSharp.Commands.Audio.Fairlight.Master;

[Command("FMPP")]
internal partial class FairlightMixerMasterPropertiesUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private bool _audioFollowsVideo;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.GetFairlight().Master.AudioFollowsVideo = AudioFollowsVideo;
    }
}
