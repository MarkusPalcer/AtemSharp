using System.Text;
using AtemSharp.State;

namespace AtemSharp.Commands.DeviceProfile;

/// <summary>
/// Mix effect block configuration command received from ATEM
/// </summary>
[Command("_MeC")]
public class MixEffectBlockConfigCommand : IDeserializedCommand
{
    /// <summary>
    /// Mix effect index
    /// </summary>
    public byte Index { get; set; }

    /// <summary>
    /// Number of keyers available in this mix effect
    /// </summary>
    public byte KeyCount { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <returns>Deserialized command instance</returns>
    public static MixEffectBlockConfigCommand Deserialize(Stream stream)
    {
        using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);

        var index = reader.ReadByte();
        var keyCount = reader.ReadByte();

        return new MixEffectBlockConfigCommand
        {
            Index = index,
            KeyCount = keyCount
        };
    }

    /// <summary>
    /// Apply the command's values to the ATEM state
    /// </summary>
    /// <param name="state">Current ATEM state to update</param>
    /// <returns>Paths indicating what was changed in the state</returns>
    public string[] ApplyToState(AtemState state)
    {
        // Update device info mix effect configuration
        state.Info.MixEffects[Index] = new MixEffectInfo
        {
            KeyCount = KeyCount
        };

        return [$"info.mixEffects.{Index}"];
    }
}