using System.Text;
using AtemSharp.State;

namespace AtemSharp.Commands.Audio;

/// <summary>
/// Update command for audio mixer properties
/// </summary>
[Command("AMPP")]
public class AudioMixerPropertiesUpdateCommand : DeserializedCommand
{
    /// <summary>
    /// Whether audio follows video crossfade transition
    /// </summary>
    public bool AudioFollowVideo { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <returns>Deserialized command instance</returns>
    public static AudioMixerPropertiesUpdateCommand Deserialize(Stream stream)
    {
        using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);

        var audioFollowVideo = reader.ReadBoolean();

        return new AudioMixerPropertiesUpdateCommand
        {
            AudioFollowVideo = audioFollowVideo
        };
    }

    /// <summary>
    /// Apply the command's values to the ATEM state
    /// </summary>
    /// <param name="state">Current ATEM state to update</param>
    /// <returns>Path indicating what was changed in the state</returns>
    /// <exception cref="InvalidIdError">Thrown if classic audio is not available</exception>
    public override string[] ApplyToState(AtemState state)
    {
        if (state.Audio == null)
        {
            throw new InvalidIdError("Classic Audio", "properties");
        }

        // Update property
        state.Audio.AudioFollowVideoCrossfadeTransitionEnabled = AudioFollowVideo;

        return ["audio.audioFollowVideoCrossfadeTransitionEnabled"];
    }
}