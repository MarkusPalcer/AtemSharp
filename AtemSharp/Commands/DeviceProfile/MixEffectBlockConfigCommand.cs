using AtemSharp.Enums;
using AtemSharp.Helpers;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.DeviceProfile;

/// <summary>
/// Mix effect block configuration command received from ATEM
/// </summary>
[Command("_MeC")]
public partial class MixEffectBlockConfigCommand : IDeserializedCommand
{
    /// <summary>
    /// Mix effect index
    /// </summary>
    [DeserializedField(0)]
    private byte _index;

    /// <summary>
    /// Number of keyers available in this mix effect
    /// </summary>
    [DeserializedField(1)]
    private  byte _keyCount;

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
