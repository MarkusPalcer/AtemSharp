using AtemSharp.Enums.Fairlight;
using AtemSharp.Enums.Ports;

namespace AtemSharp.State.Audio.Fairlight;

public class FairlightAudioInputProperties
{
    public FairlightInputType InputType { get; set; }
    public ExternalPortType ExternalPortType { get; set; }
    public FairlightInputConfiguration[] SupportedConfigurations { get; set; } = [];
    public FairlightInputConfiguration ActiveConfiguration { get; set; }
    public FairlightAnalogInputLevel[] SupportedInputLevels { get; set; } = [];
    public FairlightAnalogInputLevel ActiveInputLevel { get; set; }
    public bool RcaToXlrEnabled { get; set; }
}
