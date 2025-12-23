using AtemSharp.State;

namespace AtemSharp.Commands.DeviceProfile;

[Command("_MeC")]
internal partial class MixEffectBlockConfigCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _index;
    [DeserializedField(1)] private byte _keyCount;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var item = state.Info.MixEffects[Index];
        item.KeyCount = KeyCount;
    }
}
