using AtemSharp.State.Audio.ClassicAudio;

namespace AtemSharp.Commands.Audio.ClassicAudio;

/// <summary>
/// Command to update audio mixer properties
/// </summary>
[Command("CAMP")]
[BufferSize(4)]
public partial class AudioMixerPropertiesCommand(ClassicAudioState audio) : SerializedCommand
{
    /// <summary>
    /// Whether audio follows video crossfade transition
    /// </summary>
    [SerializedField(1, 0)] private bool _audioFollowVideo = audio.AudioFollowsVideo;
}
