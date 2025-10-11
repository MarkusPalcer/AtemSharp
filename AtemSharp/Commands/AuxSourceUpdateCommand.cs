using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands;

/// <summary>
/// Command received from ATEM device containing auxiliary source update
/// </summary>
[Command("AuxS")]
public class AuxSourceUpdateCommand : IDeserializedCommand
{
	/// <summary>
	/// Auxiliary output index (0-based)
	/// </summary>
	public int AuxBus { get; set; }

	/// <summary>
	/// Source input number for the auxiliary output
	/// </summary>
	public int Source { get; set; }

	/// <summary>
	/// Deserialize the command from binary stream
	/// </summary>
	/// <param name="stream">Binary stream containing command data</param>
	/// <param name="protocolVersion">Protocol version used for deserialization</param>
	/// <returns>Deserialized command instance</returns>
	public static AuxSourceUpdateCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
	{
		using var reader = new BinaryReader(stream, System.Text.Encoding.Default, leaveOpen: true);

		var auxBus = reader.ReadByte();
		reader.ReadByte(); // Skip padding byte
		var source = reader.ReadUInt16BigEndian();

		return new AuxSourceUpdateCommand
		{
			AuxBus = auxBus,
			Source = source
		};
	}

	/// <inheritdoc />
	public string[] ApplyToState(AtemState state)
	{
		// Validate auxiliary output index
		if (state.Info.Capabilities == null || AuxBus >= state.Info.Capabilities.Auxiliaries)
		{
			throw new InvalidIdError("Auxiliary", AuxBus);
		}

		// Ensure Video state exists
		state.Video ??= new VideoState();

		// Ensure auxiliaries array is large enough
		if (state.Video.Auxiliaries.Length <= AuxBus)
		{
			var newArray = new int?[AuxBus + 1];
			Array.Copy(state.Video.Auxiliaries, newArray, state.Video.Auxiliaries.Length);
			state.Video.Auxiliaries = newArray;
		}

		// Update the auxiliary source
		state.Video.Auxiliaries[AuxBus] = Source;

		// Return the state path that was modified
		return [$"video.auxiliaries.{AuxBus}"];
	}
}