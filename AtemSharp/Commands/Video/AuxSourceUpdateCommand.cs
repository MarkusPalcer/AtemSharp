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
    [DeserializedField(0)]
    private byte _auxBus;

    /// <summary>
    /// Source input number for the auxiliary output
    /// </summary>
    [DeserializedField(2)]
    private ushort _source;

	/// <inheritdoc />
	public void ApplyToState(AtemState state)
	{
		// Validate auxiliary output index
		if (state.Info.Capabilities is null || AuxBus >= state.Info.Capabilities.Auxiliaries)
		{
			throw new InvalidIdError("Auxiliary", AuxBus);
		}

		// Update the auxiliary source
		state.Video.Auxiliaries[AuxBus] = Source;
	}
}
