using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;
using AtemSharp.State.Info;
using AtemSharp.State.Ports;

namespace AtemSharp.Commands.Audio.Fairlight;

[Command("FAIP", ProtocolVersion.V8_1_1)]
internal partial class FairlightMixerInputUpdateV811Command : IDeserializedCommand
{
    [DeserializedField(0)] private ushort _id;
    [DeserializedField(2)] private FairlightInputType _inputType;
    [DeserializedField(6)] private ExternalPortType _externalPortType;

    [DeserializedField(9)]
    [CustomScaling($"{nameof(DeserializationExtensions)}.{nameof(DeserializationExtensions.GetComponents)}")]
    [SerializedType(typeof(FairlightInputConfiguration))]
    private FairlightInputConfiguration[] _supportedConfigurations = [];

    [DeserializedField(10)] private FairlightInputConfiguration _activeConfiguration;

    [DeserializedField(11)]
    [CustomScaling($"{nameof(DeserializationExtensions)}.{nameof(DeserializationExtensions.GetComponents)}")]
    [SerializedType(typeof(FairlightAnalogInputLevel))]
    private FairlightAnalogInputLevel[] _supportedInputLevels = [];

    [DeserializedField(12)] private FairlightAnalogInputLevel _activeInputLevel;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var fairlightAudioInput = state.GetFairlight().Inputs.GetOrCreate(Id);
        fairlightAudioInput.Id = Id;
        fairlightAudioInput.InputType = InputType;
        fairlightAudioInput.ExternalPortType = ExternalPortType;
        fairlightAudioInput.SupportedConfigurations = SupportedConfigurations;
        fairlightAudioInput.ActiveConfiguration = ActiveConfiguration;
        fairlightAudioInput.SupportedInputLevels = SupportedInputLevels;
        fairlightAudioInput.ActiveInputLevel = ActiveInputLevel;
    }
}
