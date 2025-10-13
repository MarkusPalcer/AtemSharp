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
    public int InputId { get; set; }

    /// <summary>
    /// Long descriptive name for the input
    /// </summary>
    public string LongName { get; set; } = string.Empty;

    /// <summary>
    /// Short name for the input
    /// </summary>
    public string ShortName { get; set; } = string.Empty;

    /// <summary>
    /// Whether the names are using default values
    /// </summary>
    public bool AreNamesDefault { get; set; }

    /// <summary>
    /// Available external port types for this input
    /// TODO: Investigate relationship between this property and test data field "AvailableExternalPorts"
    /// - Test data contains "AvailableExternalPorts" as ushort (raw flag value)
    /// - This property contains parsed ExternalPortType[] array
    /// - Need to determine if this should be derived from the raw value or if both serve different purposes
    /// - TypeScript original uses Util.getComponents() to convert raw value to array
    /// </summary>
    public ExternalPortType[]? ExternalPorts { get; set; }

    /// <summary>
    /// Current external port type being used
    /// </summary>
    public ExternalPortType ExternalPortType { get; set; }

    /// <summary>
    /// Internal port type for this input
    /// </summary>
    public InternalPortType InternalPortType { get; set; }

    /// <summary>
    /// Source availability flags indicating where this input can be used
    /// </summary>
    public SourceAvailability SourceAvailability { get; set; }

    /// <summary>
    /// Mix effect availability flags indicating which MEs can use this input
    /// </summary>
    public MeAvailability MeAvailability { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <param name="protocolVersion">Protocol version used for deserialization</param>
    /// <returns>Deserialized command instance</returns>
    public static InputPropertiesUpdateCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, System.Text.Encoding.Default, leaveOpen: true);

        var inputId = reader.ReadUInt16BigEndian();
        
        // Read strings
        var longNameBytes = reader.ReadBytes(20);
        var longName = AtemUtil.ReadNullTerminatedString(longNameBytes, 0, 20);
        
        var shortNameBytes = reader.ReadBytes(4);
        var shortName = AtemUtil.ReadNullTerminatedString(shortNameBytes, 0, 4);
        
        var areNamesDefault = reader.ReadBoolean();;
        reader.ReadByte(); // Skip padding
        
        var externalPortsValue = reader.ReadUInt16BigEndian();
        var portComponents = AtemUtil.GetComponents((ExternalPortType)externalPortsValue);
        var externalPorts = portComponents.Length == 0 ? null : portComponents;
        
        var externalPortType = (ExternalPortType)reader.ReadUInt16BigEndian();
        var internalPortType = (InternalPortType)reader.ReadByte();
        reader.ReadByte(); // Skip padding
        
        var sourceAvailability = (SourceAvailability)reader.ReadByte();
        var meAvailability = (MeAvailability)reader.ReadByte();

        return new InputPropertiesUpdateCommand
        {
            InputId = inputId,
            LongName = longName,
            ShortName = shortName,
            AreNamesDefault = areNamesDefault,
            ExternalPorts = externalPorts,
            ExternalPortType = externalPortType,
            InternalPortType = internalPortType,
            SourceAvailability = sourceAvailability,
            MeAvailability = meAvailability
        };
    }

    /// <inheritdoc />
    public string[] ApplyToState(AtemState state)
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

        // Return the state path that was modified
        return [$"video.inputs.{InputId}"];
    }
}