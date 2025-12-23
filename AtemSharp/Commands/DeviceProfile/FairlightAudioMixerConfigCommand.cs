using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;
using AtemSharp.State.Info;

namespace AtemSharp.Commands.DeviceProfile;

[Command("_FAC")]
internal partial class FairlightAudioMixerConfigCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _inputs;
    [DeserializedField(1)] private byte _monitors;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Update device info fairlight mixer configuration
        state.Info.Mixer = new FairlightAudioMixerInfo
        {
            Inputs = Inputs,
            Monitors = Monitors
        };

        // Initialize fairlight state with the received configuration
        state.Audio = new FairlightAudioState();
    }
}
