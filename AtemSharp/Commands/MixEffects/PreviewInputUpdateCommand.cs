using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects;

[Command("PrvI")]
internal partial class PreviewInputUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _mixEffectId;

    [DeserializedField(2)] private ushort _source;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.Video.MixEffects[MixEffectId].PreviewInput = Source;
    }
}
