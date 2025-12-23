using AtemSharp.State;
using AtemSharp.State.Ports;
using AtemSharp.State.Video.InputChannel;

namespace AtemSharp.Commands.Inputs;

[Command("InPr")]
internal partial class InputPropertiesUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private ushort _inputId;

    // Stryker disable once string : initialization is always overriden by deserialization
    [CustomDeserialization] private string _longName = string.Empty;

    // Stryker disable once string : initialization is always overriden by deserialization
    [CustomDeserialization] private string _shortName = string.Empty;

    [DeserializedField(26)] private bool _areNamesDefault;

    [DeserializedField(28)]
    [SerializedType(typeof(ExternalPortType))]
    [CustomScaling($"{nameof(DeserializationExtensions)}.{nameof(DeserializationExtensions.GetComponents)}")]
    private ExternalPortType[] _externalPorts = [];

    [DeserializedField(30)] private ExternalPortType _externalPortType;

    [DeserializedField(32)] private InternalPortType _internalPortType;

    [DeserializedField(34)] private SourceAvailability _sourceAvailability;

    [DeserializedField(35)] private MeAvailability _meAvailability;

    private void DeserializeInternal(ReadOnlySpan<byte> rawCommand)
    {
        LongName = rawCommand.ReadString(2, 20);
        ShortName = rawCommand.ReadString(22, 4);
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var inputChannel = state.Video.Inputs.GetOrCreate(InputId);
        inputChannel.LongName = LongName;
        inputChannel.ShortName = ShortName;
        inputChannel.AreNamesDefault = AreNamesDefault;
        inputChannel.ExternalPorts = ExternalPorts;
        inputChannel.ExternalPortType = ExternalPortType;
        inputChannel.InternalPortType = InternalPortType;
        inputChannel.SourceAvailability = SourceAvailability;
        inputChannel.MeAvailability = MeAvailability;
    }
}
