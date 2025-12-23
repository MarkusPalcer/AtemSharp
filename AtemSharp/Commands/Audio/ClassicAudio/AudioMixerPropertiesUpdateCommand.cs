using AtemSharp.State;

namespace AtemSharp.Commands.Audio.ClassicAudio;

[Command("AMPP")]
internal partial class AudioMixerPropertiesUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private bool _audioFollowVideo;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.GetClassicAudio().AudioFollowsVideo = AudioFollowVideo;
    }
}
