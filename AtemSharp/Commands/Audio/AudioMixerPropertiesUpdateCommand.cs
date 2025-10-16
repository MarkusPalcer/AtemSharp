using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;
using AtemSharp.State.Audio.ClassicAudio;

namespace AtemSharp.Commands.Audio;

/// <summary>
/// Update command for audio mixer properties
/// </summary>
[Command("AMPP")]
public class AudioMixerPropertiesUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Whether audio follows video crossfade transition
    /// </summary>
    public bool AudioFollowVideo { get; init; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <returns>Deserialized command instance</returns>
    public static AudioMixerPropertiesUpdateCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new AudioMixerPropertiesUpdateCommand
        {
            AudioFollowVideo = rawCommand.ReadBoolean(0)
        };
    }

    /// <summary>
    /// Apply the command's values to the ATEM state
    /// </summary>
    /// <param name="state">Current ATEM state to update</param>
    /// <returns>Path indicating what was changed in the state</returns>
    /// <exception cref="InvalidIdError">Thrown if classic audio is not available</exception>
    public void ApplyToState(AtemState state)
    {
        if (state.Audio is not ClassicAudioState audio)
        {
            throw new InvalidOperationException("Cannot apply AudioMixerPropertiesUpdateCommand to non-classic audio state");
        }

        audio.AudioFollowsVideo = AudioFollowVideo;
    }
}
