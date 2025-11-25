using AtemSharp.State;

namespace AtemSharp.Commands.Video;

/// <summary>
/// Command received from ATEM device containing auxiliary source update
/// </summary>
[Command("AuxS")]
public partial class AuxSourceUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Auxiliary output index (0-based)
    /// </summary>
    [DeserializedField(0)] private byte _auxId;

    /// <summary>
    /// Source input number for the auxiliary output
    /// </summary>
    [DeserializedField(2)] private ushort _source;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.Video.Auxiliaries[AuxId].Source = Source;
    }
}
