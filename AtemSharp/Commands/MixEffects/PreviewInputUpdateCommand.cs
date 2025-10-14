using AtemSharp.Enums;
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
	public int MixEffectId { get; set; }

	/// <summary>
	/// Preview input source number
	/// </summary>
	public int Source { get; set; }

	/// <summary>
	/// Deserialize the command from binary stream
	/// </summary>
	/// <param name="stream">Binary stream containing command data</param>
	/// <param name="protocolVersion">Protocol version used for deserialization</param>
	/// <returns>Deserialized command instance</returns>
	public static PreviewInputUpdateCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
	{
		using var reader = new BinaryReader(stream, System.Text.Encoding.Default, leaveOpen: true);

		var mixEffectId = reader.ReadByte();
		reader.ReadByte(); // Skip padding byte
		var source = reader.ReadUInt16BigEndian();

		return new PreviewInputUpdateCommand
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

		// Update the preview input
		mixEffect.PreviewInput = Source;
	}
}
