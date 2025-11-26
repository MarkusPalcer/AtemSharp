using AtemSharp.State;
using AtemSharp.State.Audio.ClassicAudio;
using AtemSharp.State.Info;

namespace AtemSharp.Commands.DeviceProfile;

/// <summary>
/// Audio mixer configuration command received from ATEM
/// </summary>
[Command("_AMC")]
public partial class AudioMixerConfigCommand : IDeserializedCommand
{
    /// <summary>
    /// Number of audio inputs available
    /// </summary>
    [DeserializedField(0)] private byte _inputs;

    /// <summary>
    /// Number of monitor channels available
    /// </summary>
    [DeserializedField(1)] private byte _monitors;

    /// <summary>
    /// Number of headphone channels available
    /// </summary>
    [DeserializedField(2)] private byte _headphones;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Update device info audio mixer configuration
        state.Info.Mixer = new AudioMixerInfo
        {
            Inputs = Inputs,
            Monitors = Monitors,
            Headphones = Headphones
        };

        // Initialize audio state
        state.Audio ??= new ClassicAudioState();
    }
}
