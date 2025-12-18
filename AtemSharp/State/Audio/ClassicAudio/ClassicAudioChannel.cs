using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using AtemSharp.State.Ports;

namespace AtemSharp.State.Audio.ClassicAudio;

/// <summary>
/// Classic audio channel properties
/// </summary>
[ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
[DebuggerDisplay("{" + nameof(ToString) + ",nq}")]
public class ClassicAudioChannel
{
    public ushort Id { get; init; }

    /// <summary>
    /// Audio source type (readonly)
    /// </summary>
    public AudioSourceType SourceType { get; internal set; }

    /// <summary>
    /// External port type
    /// </summary>
    public ExternalPortType PortType { get; internal set; }

    /// <summary>
    /// Audio mix option
    /// </summary>
    public AudioMixOption MixOption { get; internal set; }

    /// <summary>
    /// Gain in decibel, -Infinity to +6dB
    /// </summary>
    public double Gain { get; internal set; }

    /// <summary>
    /// Balance, -50 to +50
    /// </summary>
    public double Balance { get; internal set; }

    /// <summary>
    /// Whether this channel supports RCA to XLR enabled setting (readonly)
    /// </summary>
    public bool SupportsRcaToXlrEnabled { get; internal set; }

    /// <summary>
    /// RCA to XLR enabled
    /// </summary>
    public bool RcaToXlrEnabled { get; internal set; }

    [ExcludeFromCodeCoverage]
    public override string ToString() => $"{GetType().Name} #{Id}";
}
