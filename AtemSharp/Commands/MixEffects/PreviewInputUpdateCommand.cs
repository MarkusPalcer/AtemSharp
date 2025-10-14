using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects;

/// <summary>
/// Command received from ATEM device containing preview input update
/// </summary>
[Command("PrvI")]
public class PreviewInputUpdateCommand : IDeserializedCommand
{
	/// <summary>
	/// Mix effect index (0-based)
	/// </summary>
	public int MixEffectId { get; init; }

	/// <summary>
	/// Preview input source number
	/// </summary>
	public int Source { get; init; }

	/// <summary>
	/// Deserialize the command from binary stream
	/// </summary>
	public static PreviewInputUpdateCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
	{
        return new PreviewInputUpdateCommand
		{
			MixEffectId = rawCommand.ReadUInt8(0),
			Source = rawCommand.ReadUInt16BigEndian(2)
		};
	}

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
