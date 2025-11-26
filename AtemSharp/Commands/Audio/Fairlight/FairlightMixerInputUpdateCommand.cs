using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;
using AtemSharp.State.Info;
using AtemSharp.State.Ports;

namespace AtemSharp.Commands.Audio.Fairlight;

[Command("FAIP")]
public class FairlightMixerInputUpdateCommand : IDeserializedCommand
{
    public ushort Id { get; set; }
    public FairlightInputType InputType { get; set; }
    public ExternalPortType ExternalPortType { get; set; }
    public FairlightInputConfiguration[] SupportedConfigurations { get; set; } = [];
    public FairlightInputConfiguration ActiveConfiguration { get; set; }
    public FairlightAnalogInputLevel[] SupportedInputLevels { get; set; } = [];
    public FairlightAnalogInputLevel ActiveInputLevel { get; set; }

    public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        var rcaToXlr = protocolVersion < ProtocolVersion.V8_1_1;

        return new FairlightMixerInputUpdateCommand
        {
            Id = rawCommand.ReadUInt16BigEndian(0),
            InputType = (FairlightInputType)rawCommand.ReadUInt8(2),
            ExternalPortType = (ExternalPortType)rawCommand.ReadUInt16BigEndian(6),
            SupportedConfigurations =
                DeserializationExtensions.GetComponents((FairlightInputConfiguration)rawCommand.ReadUInt8(rcaToXlr ? 11 : 9)),
            ActiveConfiguration = (FairlightInputConfiguration)rawCommand.ReadUInt8(rcaToXlr ? 12 : 10),
            SupportedInputLevels = rcaToXlr
                                       ? (rawCommand.ReadBoolean(9)
                                              ? [FairlightAnalogInputLevel.ProLine, FairlightAnalogInputLevel.Microphone]
                                              : [])
                                       : DeserializationExtensions.GetComponents((FairlightAnalogInputLevel)rawCommand.ReadUInt8(11)),
            ActiveInputLevel = rcaToXlr
                                   ? rawCommand.ReadUInt8(9) > 0
                                         ? FairlightAnalogInputLevel.ProLine
                                         : FairlightAnalogInputLevel.Microphone
                                   : (FairlightAnalogInputLevel)rawCommand.ReadUInt8(12),
        };
    }

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
