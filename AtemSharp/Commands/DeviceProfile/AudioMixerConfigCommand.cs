using System.Text;
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
    public byte Inputs { get; set; }

    /// <summary>
    /// Number of monitor channels available
    /// </summary>
    public byte Monitors { get; set; }

    /// <summary>
    /// Number of headphone channels available
    /// </summary>
    public byte Headphones { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <returns>Deserialized command instance</returns>
    public static AudioMixerConfigCommand Deserialize(Stream stream)
    {
        using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);

        var inputs = reader.ReadByte();
        var monitors = reader.ReadByte();
        var headphones = reader.ReadByte();

        return new AudioMixerConfigCommand
        {
            Inputs = inputs,
            Monitors = monitors,
            Headphones = headphones
        };
    }

    /// <summary>
    /// Apply the command's values to the ATEM state
    /// </summary>
    /// <param name="state">Current ATEM state to update</param>
    /// <returns>Paths indicating what was changed in the state</returns>
    public string[] ApplyToState(AtemState state)
    {
        // Update device info audio mixer configuration
        state.Info.AudioMixer = new AudioMixerInfo
        {
            Inputs = Inputs,
            Monitors = Monitors,
            Headphones = Headphones
        };

        // Initialize audio state with the received configuration
        state.Audio = new AudioState
        {
            Channels = new Dictionary<int, ClassicAudioChannel>()
        };

        return ["info.audioMixer", "audio"];
    }
}