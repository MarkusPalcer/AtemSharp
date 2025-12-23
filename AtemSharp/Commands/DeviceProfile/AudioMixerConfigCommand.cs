using AtemSharp.State;
using AtemSharp.State.Audio.ClassicAudio;
using AtemSharp.State.Info;

namespace AtemSharp.Commands.DeviceProfile;

[Command("_AMC")]
internal partial class AudioMixerConfigCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _inputs;
    [DeserializedField(1)] private byte _monitors;
    [DeserializedField(2)] private byte _headphones;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Update device info audio mixer configuration
        state.Info.Mixer = new ClassicAudioMixerInfo
        {
            Inputs = Inputs,
            Monitors = Monitors,
            Headphones = Headphones
        };

        // Initialize audio state
        state.Audio = new ClassicAudioState();
    }
}
