using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects;

/// <summary>
/// Command received from ATEM device containing program input update
/// </summary>
[Command("PrgI")]
public class ProgramInputUpdateCommand : IDeserializedCommand
{
	/// <summary>
	/// Mix effect index (0-based)
	/// </summary>
	public int MixEffectId { get; init; }

	/// <summary>
	/// Program input source number
	/// </summary>
	public int Source { get; init; }

	/// <summary>
	/// Deserialize the command from binary stream
	/// </summary>
	public static ProgramInputUpdateCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
	{
		var mixEffectId = rawCommand.ReadUInt8(0);
		var source = rawCommand.ReadUInt16BigEndian(2);

		return new ProgramInputUpdateCommand
		{
			MixEffectId = mixEffectId,
			Source = source
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

		// Update the program input
		mixEffect.ProgramInput = Source;
	}
}
