using AtemSharp.Helpers;
using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.DeviceProfile;

/// <summary>
/// Fairlight audio mixer configuration command received from ATEM
/// </summary>
[Command("_FAC")]
public partial class FairlightAudioMixerConfigCommand : IDeserializedCommand
{
    /// <summary>
    /// Number of Fairlight audio inputs available
    /// </summary>
    [DeserializedField(0)]
    private byte _inputs;

    /// <summary>
    /// Number of Fairlight monitor channels available
    /// </summary>
    [DeserializedField(1)]
    private byte _monitors;

    /// <summary>
    /// Apply the command's values to the ATEM state
    /// </summary>
    /// <param name="state">Current ATEM state to update</param>
    /// <returns>Paths indicating what was changed in the state</returns>
    public void ApplyToState(AtemState state)
    {
        // Update device info fairlight mixer configuration
        state.Info.FairlightMixer = new FairlightAudioMixerInfo
        {
            Inputs = Inputs,
            Monitors = Monitors
        };

        // Initialize fairlight state with the received configuration
        state.Audio = new FairlightAudioState();
    }
}
