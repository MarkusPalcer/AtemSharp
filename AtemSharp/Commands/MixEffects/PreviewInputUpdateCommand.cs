using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects;

/// <summary>
/// Command received from ATEM device containing preview input update
/// </summary>
[Command("PrvI")]
public partial class PreviewInputUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private byte _mixEffectId;

    /// <summary>
    /// Preview input source number
    /// </summary>
    [DeserializedField(2)]
    private ushort _source;

	/// <inheritdoc />
	public void ApplyToState(AtemState state)
	{
		// Validate mix effect index
		if (state.Info.Capabilities == null || MixEffectId >= state.Info.Capabilities.MixEffects)
		{
			throw new InvalidIdError("MixEffect", MixEffectId);
		}

		// Get or create the mix effect
		var mixEffect = state.Video.MixEffects.GetOrCreate(MixEffectId);

		// Update the preview input
		mixEffect.PreviewInput = Source;
	}
}
