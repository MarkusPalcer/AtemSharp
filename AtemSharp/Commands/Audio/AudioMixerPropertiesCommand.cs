using AtemSharp.Helpers;
using AtemSharp.State;

namespace AtemSharp.Commands.Audio;

/// <summary>
/// Command to update audio mixer properties
/// </summary>
[Command("CAMP")]
[BufferSize(4)]
public partial class AudioMixerPropertiesCommand : SerializedCommand
{
    /// <summary>
    /// Whether audio follows video crossfade transition
    /// </summary>
    [SerializedField(1,0)]
    private bool _audioFollowVideo;

    public AudioMixerPropertiesCommand(AtemState currentState)
    {
        var audio = currentState.GetClassicAudio();
        _audioFollowVideo = audio.AudioFollowsVideo ?? false;
    }
}
