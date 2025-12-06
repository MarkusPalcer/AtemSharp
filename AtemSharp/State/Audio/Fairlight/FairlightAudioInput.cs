using System.Diagnostics.CodeAnalysis;
using AtemSharp.State.Ports;

namespace AtemSharp.State.Audio.Fairlight;

/// <summary>
/// Fairlight audio input state
/// </summary>
[ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
public class FairlightAudioInput : ItemWithId<ushort>
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

    internal override void SetId(ushort id) => Id = id;
}
