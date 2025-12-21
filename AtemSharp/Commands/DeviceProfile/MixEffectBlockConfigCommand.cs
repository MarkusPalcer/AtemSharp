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
    [DeserializedField(0)] private byte _index;

    /// <summary>
    /// Number of keyers available in this mix effect
    /// </summary>
    [DeserializedField(1)] private byte _keyCount;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var item = state.Info.MixEffects[Index];
        item.KeyCount = KeyCount;
    }
}
