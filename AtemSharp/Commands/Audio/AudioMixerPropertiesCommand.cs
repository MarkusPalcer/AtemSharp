using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;
using AtemSharp.State.Audio.ClassicAudio;

namespace AtemSharp.Commands.Audio;

/// <summary>
/// Command to update audio mixer properties
/// </summary>
[Command("CAMP")]
public class AudioMixerPropertiesCommand : SerializedCommand
{
    private bool _audioFollowVideo;

    /// <summary>
    /// Create command initialized with current state values
    /// </summary>
    /// <param name="currentState">Current ATEM state</param>
    /// <exception cref="InvalidIdError">Thrown if classic audio not available</exception>
    public AudioMixerPropertiesCommand(AtemState currentState)
    {
        if (currentState.Audio is not ClassicAudioState audio)
        {
            throw new InvalidOperationException("Classic audio state is not available");
        }

        // Initialize from current state (direct field access = no flags set)
        _audioFollowVideo = audio.AudioFollowsVideo ?? false;
    }

    /// <summary>
    /// Whether audio follows video crossfade transition
    /// </summary>
    public bool AudioFollowVideo
    {
        get => _audioFollowVideo;
        set
        {
            _audioFollowVideo = value;
            Flag |= 1 << 0;
        }
    }

    /// <summary>
    /// Serialize command to binary stream for transmission to ATEM
    /// </summary>
    /// <param name="version">Protocol version</param>
    /// <returns>Serialized command data as byte array</returns>
    public override byte[] Serialize(ProtocolVersion version)
    {
        using var memoryStream = new MemoryStream(4);
        using var writer = new BinaryWriter(memoryStream);

        writer.Write((byte)Flag);
        writer.WriteBoolean(AudioFollowVideo);
        writer.Pad(2);

        return memoryStream.ToArray();
    }
}
