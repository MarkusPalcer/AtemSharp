using AtemSharp.Helpers;
using AtemSharp.State;

namespace AtemSharp.Commands.Audio;

/// <summary>
/// Update command for audio mixer properties
/// </summary>
[Command("AMPP")]
public partial class AudioMixerPropertiesUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Whether audio follows video crossfade transition
    /// </summary>
    [DeserializedField(0)]
    private bool _audioFollowVideo;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.GetClassicAudio().AudioFollowsVideo = AudioFollowVideo;
    }
}
