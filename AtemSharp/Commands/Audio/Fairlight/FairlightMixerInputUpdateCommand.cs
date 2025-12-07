using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;
using AtemSharp.State.Ports;

namespace AtemSharp.Commands.Audio.Fairlight;

[Command("FAIP")]
public partial class FairlightMixerInputUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private ushort _id;
    [DeserializedField(2)] private FairlightInputType _inputType;
    [DeserializedField(6)] private ExternalPortType _externalPortType;

    [DeserializedField(11)]
    [CustomScaling($"{nameof(DeserializationExtensions)}.{nameof(DeserializationExtensions.GetComponents)}")]
    [SerializedType(typeof(FairlightInputConfiguration))]
    private FairlightInputConfiguration[] _supportedConfigurations = [];

    [DeserializedField(12)] private FairlightInputConfiguration _activeConfiguration;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var fairlightAudioInput = state.GetFairlight().Inputs.GetOrCreate(Id);
        fairlightAudioInput.Id = Id;
        fairlightAudioInput.InputType = InputType;
        fairlightAudioInput.ExternalPortType = ExternalPortType;
        fairlightAudioInput.SupportedConfigurations = SupportedConfigurations;
        fairlightAudioInput.ActiveConfiguration = ActiveConfiguration;
        fairlightAudioInput.SupportedInputLevels = [];
        fairlightAudioInput.ActiveInputLevel = 0;
    }
}
