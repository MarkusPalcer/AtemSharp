using AtemSharp.Enums;
using AtemSharp.Lib;
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
    public byte Index { get; init; }

    /// <summary>
    /// Number of keyers available in this mix effect
    /// </summary>
    public byte KeyCount { get; init; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    public static MixEffectBlockConfigCommand Deserialize(ReadOnlySpan<Byte> rawCommand, ProtocolVersion version)
    {
        var index = rawCommand.ReadUInt8(0);
        var keyCount = rawCommand.ReadUInt8(1);

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
    public void ApplyToState(AtemState state)
    {
        // Update device info mix effect configuration
        state.Info.MixEffects[Index] = new MixEffectInfo
        {
            KeyCount = KeyCount
        };
    }
}
