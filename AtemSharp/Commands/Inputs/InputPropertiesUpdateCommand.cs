using AtemSharp.State;
using AtemSharp.State.Ports;
using AtemSharp.State.Video.InputChannel;

namespace AtemSharp.Commands.Inputs;

/// <summary>
/// Command received from ATEM device containing input properties update
/// </summary>
[Command("InPr")]
public partial class InputPropertiesUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Input identifier/number
    /// </summary>
    [DeserializedField(0)] private ushort _inputId;

    // Stryker disable once string : initialization is always overriden by deserialization
    /// <summary>
    /// Long descriptive name for the input
    /// </summary>
    [CustomDeserialization] private string _longName = string.Empty;

    // Stryker disable once string : initialization is always overriden by deserialization
    /// <summary>
    /// Short name for the input
    /// </summary>
    [CustomDeserialization] private string _shortName = string.Empty;

    /// <summary>
    /// Whether the names are using default values
    /// </summary>
    [DeserializedField(26)] private bool _areNamesDefault;

    /// <summary>
    /// Available external port types for this input
    /// </summary>
    [DeserializedField(28)]
    [SerializedType(typeof(ExternalPortType))]
    [CustomScaling($"{nameof(DeserializationExtensions)}.{nameof(DeserializationExtensions.GetComponents)}")]
    private ExternalPortType[]? _externalPorts;

    /// <summary>
    /// Current external port type being used
    /// </summary>
    [DeserializedField(30)] private ExternalPortType _externalPortType;

    /// <summary>
    /// Internal port type for this input
    /// </summary>
    [DeserializedField(32)] private InternalPortType _internalPortType;

    /// <summary>
    /// Source availability flags indicating where this input can be used
    /// </summary>
    [DeserializedField(34)] private SourceAvailability _sourceAvailability;

    /// <summary>
    /// Mix effect availability flags indicating which MEs can use this input
    /// </summary>
    [DeserializedField(35)] private MeAvailability _meAvailability;

    private void DeserializeInternal(ReadOnlySpan<byte> rawCommand)
    {
        LongName = rawCommand.ReadString(2, 20);
        ShortName = rawCommand.ReadString(22, 4);
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
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
