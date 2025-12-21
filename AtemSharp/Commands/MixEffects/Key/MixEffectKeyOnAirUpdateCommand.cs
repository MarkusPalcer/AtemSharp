using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Key;

/// <summary>
/// Command received from ATEM device containing upstream keyer on-air state update
/// </summary>
[Command("KeOn")]
public partial class MixEffectKeyOnAirUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _mixEffectId;

    [DeserializedField(1)] private byte _keyerId;

    /// <summary>
    /// Whether the upstream keyer is on air
    /// </summary>
    [DeserializedField(2)] private bool _onAir;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var mixEffect = state.Video.MixEffects[MixEffectId];
        var keyer = mixEffect.UpstreamKeyers.GetOrCreate(KeyerId);

        // Update the on-air state
        keyer.OnAir = OnAir;
    }
}
