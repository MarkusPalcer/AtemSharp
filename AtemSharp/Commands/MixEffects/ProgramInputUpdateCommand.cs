using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects;

/// <summary>
/// Command received from ATEM device containing program input update
/// </summary>
[Command("PrgI")]
public partial class ProgramInputUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _mixEffectId;

    /// <summary>
    /// Program input source number
    /// </summary>
    [DeserializedField(2)] private ushort _source;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.Video.MixEffects[MixEffectId].ProgramInput = Source;
    }
}
