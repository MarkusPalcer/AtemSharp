using System.Text;
using AtemSharp.Enums;
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
    public byte Inputs { get; set; }

    /// <summary>
    /// Number of Fairlight monitor channels available
    /// </summary>
    public byte Monitors { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <returns>Deserialized command instance</returns>
    public static FairlightAudioMixerConfigCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);

        var inputs = reader.ReadByte();
        var monitors = reader.ReadByte();

        return new FairlightAudioMixerConfigCommand
        {
            Inputs = inputs,
            Monitors = monitors
        };
    }

    /// <summary>
    /// Apply the command's values to the ATEM state
    /// </summary>
    /// <param name="state">Current ATEM state to update</param>
    /// <returns>Paths indicating what was changed in the state</returns>
    public string[] ApplyToState(AtemState state)
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

        return ["info.fairlightMixer", "fairlight.inputs"];
    }
}