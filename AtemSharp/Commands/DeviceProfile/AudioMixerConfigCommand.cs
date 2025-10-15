using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.DeviceProfile;

/// <summary>
/// Audio mixer configuration command received from ATEM
/// </summary>
[Command("_AMC")]
public class AudioMixerConfigCommand : IDeserializedCommand
{
    /// <summary>
    /// Number of audio inputs available
    /// </summary>
    public byte Inputs { get; init; }

    /// <summary>
    /// Number of monitor channels available
    /// </summary>
    public byte Monitors { get; init; }

    /// <summary>
    /// Number of headphone channels available
    /// </summary>
    public byte Headphones { get; init; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    public static AudioMixerConfigCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new AudioMixerConfigCommand
        {
            Inputs = rawCommand.ReadUInt8(0),
            Monitors = rawCommand.ReadUInt8(1),
            Headphones = rawCommand.ReadUInt8(2)
        };
    }

    /// <summary>
    /// Apply the command's values to the ATEM state
    /// </summary>
    /// <param name="state">Current ATEM state to update</param>
    /// <returns>Paths indicating what was changed in the state</returns>
    public void ApplyToState(AtemState state)
    {
        // Update device info audio mixer configuration
        state.Info.AudioMixer = new AudioMixerInfo
        {
            Inputs = Inputs,
            Monitors = Monitors,
            Headphones = Headphones
        };

        // Initialize audio state with the received configuration
        state.Audio = new ClassicAudioState
        {
            Channels = new Dictionary<int, ClassicAudioChannel>()
        };
    }
}
