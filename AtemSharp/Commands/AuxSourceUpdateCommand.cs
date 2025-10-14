using AtemSharp.Enums;
using AtemSharp.Lib;
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
	public int AuxBus { get; init; }

	/// <summary>
	/// Source input number for the auxiliary output
	/// </summary>
	public int Source { get; init; }

	/// <summary>
	/// Deserialize the command from binary stream
	/// </summary>
	public static AuxSourceUpdateCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
	{
		return new AuxSourceUpdateCommand
		{
			AuxBus = rawCommand.ReadUInt8(0),
			Source = rawCommand.ReadUInt16BigEndian(2)
		};
	}

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
