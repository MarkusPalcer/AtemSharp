using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Audio.ClassicAudio;

/// <summary>
/// Classic audio headphone output channel properties
/// </summary>
[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class ClassicAudioHeadphoneOutputChannel
{
    /// <summary>
    /// Gain in decibel, -Infinity to +6dB
    /// </summary>
    public double Gain { get; internal set; }

    /// <summary>
    /// Program out gain in decibel, -Infinity to +6dB
    /// </summary>
    public double ProgramOutGain { get; internal set; }

    /// <summary>
    /// Sidetone gain in decibel, -Infinity to +6dB
    /// </summary>
    public double SidetoneGain { get; internal set; }

    /// <summary>
    /// Talkback gain in decibel, -Infinity to +6dB
    /// </summary>
    public double TalkbackGain { get; internal set; }
}
