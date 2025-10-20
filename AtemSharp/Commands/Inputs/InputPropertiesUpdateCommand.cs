using AtemSharp.Enums;
using AtemSharp.Enums.Ports;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Inputs;

/// <summary>
/// Command received from ATEM device containing input properties update
/// </summary>
[Command("InPr")]
public class InputPropertiesUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Input identifier/number
    /// </summary>
    public int InputId { get; init; }

    /// <summary>
    /// Long descriptive name for the input
    /// </summary>
    public string LongName { get; init; } = string.Empty;

    /// <summary>
    /// Short name for the input
    /// </summary>
    public string ShortName { get; init; } = string.Empty;

    /// <summary>
    /// Whether the names are using default values
    /// </summary>
    public bool AreNamesDefault { get; init; }

    /// <summary>
    /// Available external port types for this input
    /// TODO: Investigate relationship between this property and test data field "AvailableExternalPorts"
    /// - Test data contains "AvailableExternalPorts" as ushort (raw flag value)
    /// - This property contains parsed ExternalPortType[] array
    /// - Need to determine if this should be derived from the raw value or if both serve different purposes
    /// - TypeScript original uses Util.getComponents() to convert raw value to array
    /// </summary>
    public ExternalPortType[]? ExternalPorts { get; init; }

    /// <summary>
    /// Current external port type being used
    /// </summary>
    public ExternalPortType ExternalPortType { get; init; }

    /// <summary>
    /// Internal port type for this input
    /// </summary>
    public InternalPortType InternalPortType { get; init; }

    /// <summary>
    /// Source availability flags indicating where this input can be used
    /// </summary>
    public SourceAvailability SourceAvailability { get; init; }

    /// <summary>
    /// Mix effect availability flags indicating which MEs can use this input
    /// </summary>
    public MeAvailability MeAvailability { get; init; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    public static InputPropertiesUpdateCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        var portComponents = AtemUtil.GetComponents((ExternalPortType)rawCommand.ReadUInt16BigEndian(28));

        return new InputPropertiesUpdateCommand
        {
            InputId = rawCommand.ReadUInt16BigEndian(0),
            LongName = rawCommand.ReadString(2, 20),
            ShortName = rawCommand.ReadString(22, 4),
            AreNamesDefault = rawCommand.ReadBoolean(26),
            ExternalPorts = portComponents.Length == 0 ? null : portComponents,
            ExternalPortType = (ExternalPortType)rawCommand.ReadUInt16BigEndian(30),
            InternalPortType = (InternalPortType)rawCommand.ReadUInt8(32),
            SourceAvailability = (SourceAvailability)rawCommand.ReadUInt8(34),
            MeAvailability = (MeAvailability)rawCommand.ReadUInt8(35)
        };
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Update the input channel state
        state.Video.Inputs[InputId] = new InputChannel
        {
            InputId = InputId,
            LongName = LongName,
            ShortName = ShortName,
            AreNamesDefault = AreNamesDefault,
            ExternalPorts = ExternalPorts,
            ExternalPortType = ExternalPortType,
            InternalPortType = InternalPortType,
            SourceAvailability = SourceAvailability,
            MeAvailability = MeAvailability
        };
    }
}
