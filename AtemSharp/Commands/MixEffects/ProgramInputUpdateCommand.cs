using AtemSharp.Enums;
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
	public int MixEffectId { get; set; }

	/// <summary>
	/// Program input source number
	/// </summary>
	public int Source { get; set; }

	/// <summary>
	/// Deserialize the command from binary stream
	/// </summary>
	/// <param name="stream">Binary stream containing command data</param>
	/// <param name="protocolVersion">Protocol version used for deserialization</param>
	/// <returns>Deserialized command instance</returns>
	public static ProgramInputUpdateCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
	{
		using var reader = new BinaryReader(stream, System.Text.Encoding.Default, leaveOpen: true);

		var mixEffectId = reader.ReadByte();
		reader.ReadByte(); // Skip padding byte
		var source = reader.ReadUInt16BigEndian();

		return new ProgramInputUpdateCommand
		{
			MixEffectId = mixEffectId,
			Source = source
		};
	}

	/// <inheritdoc />
	public string[] ApplyToState(AtemState state)
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

		// Return the state path that was modified
		return [$"video.mixEffects.{MixEffectId}.programInput"];
	}
}