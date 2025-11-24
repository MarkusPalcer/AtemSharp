using AtemSharp.Enums.Fairlight;
using AtemSharp.Enums.Ports;

namespace AtemSharp.State.Audio.Fairlight;

public class FairlightAudioInputProperties
{
    public FairlightInputType InputType { get; internal set; }
    public ExternalPortType ExternalPortType { get; internal set; }
    public FairlightInputConfiguration[] SupportedConfigurations { get; internal set; } = [];
    public FairlightInputConfiguration ActiveConfiguration { get; internal set; }
    public FairlightAnalogInputLevel[] SupportedInputLevels { get; internal set; } = [];
    public FairlightAnalogInputLevel ActiveInputLevel { get; internal set; }
    public bool RcaToXlrEnabled { get; internal set; }
}
