using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.DeviceProfile;

/// <summary>
/// Fairlight audio mixer configuration command received from ATEM
/// </summary>
[Command("_FAC")]
public class FairlightAudioMixerConfigCommand : IDeserializedCommand
{
    /// <summary>
    /// Number of Fairlight audio inputs available
    /// </summary>
    public byte Inputs { get; init; }

    /// <summary>
    /// Number of Fairlight monitor channels available
    /// </summary>
    public byte Monitors { get; init; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    public static FairlightAudioMixerConfigCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new FairlightAudioMixerConfigCommand
        {
            Inputs = rawCommand.ReadUInt8(0),
            Monitors = rawCommand.ReadUInt8(1)
        };
    }

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
        state.Fairlight = new FairlightAudioState
        {
            Inputs = new Dictionary<int, FairlightAudioInput>()
        };
    }
}
