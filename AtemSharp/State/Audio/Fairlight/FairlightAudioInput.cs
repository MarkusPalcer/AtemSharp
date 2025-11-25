using AtemSharp.State.Ports;

namespace AtemSharp.State.Audio.Fairlight;

/// <summary>
/// Fairlight audio input state
/// </summary>
public class FairlightAudioInput
{
    public ushort Id { get; internal set; }

    public FairlightInputType InputType { get; internal set; }
    public ExternalPortType ExternalPortType { get; internal set; }
    public FairlightInputConfiguration[] SupportedConfigurations { get; internal set; } = [];
    public FairlightInputConfiguration ActiveConfiguration { get; internal set; }
    public FairlightAnalogInputLevel[] SupportedInputLevels { get; internal set; } = [];
    public FairlightAnalogInputLevel ActiveInputLevel { get; internal set; }
    public bool RcaToXlrEnabled { get; internal set; }

    public Dictionary<long, Source> Sources { get; } = [];
}
