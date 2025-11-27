using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Audio.Fairlight;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class MonitorProperties
{
    public double Gain { get; internal set; }
    public double InputMasterGain { get; internal set; }
    public bool InputMasterMuted { get; internal set; }
    public double InputTalkbackGain { get; internal set; }
    public bool InputTalkbackMuted { get; internal set; }
    public double InputSidetoneGain { get; internal set; }
}
