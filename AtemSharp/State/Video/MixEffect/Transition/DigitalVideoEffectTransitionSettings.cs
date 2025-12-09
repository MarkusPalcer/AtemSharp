using System.Diagnostics.CodeAnalysis;
using AtemSharp.Types;

namespace AtemSharp.State.Video.MixEffect.Transition;

/// <summary>
/// DVE transition settings for a mix effect
/// </summary>
[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class DigitalVideoEffectTransitionSettings
{
    /// <summary>
    /// Transition rate in frames
    /// </summary>
    public byte Rate { get; internal set; }

    /// <summary>
    /// Logo/key transition rate in frames
    /// </summary>
    public byte LogoRate { get; internal set; }

    /// <summary>
    /// DVE effect style
    /// </summary>
    public DigitalVideoEffect Style { get; internal set; }

    /// <summary>
    /// Fill source input number
    /// </summary>
    public ushort FillSource { get; internal set; }

    /// <summary>
    /// Key source input number
    /// </summary>
    public ushort KeySource { get; internal set; }

    /// <summary>
    /// Whether the key is enabled
    /// </summary>
    public bool EnableKey { get; internal set; }

    public PreMultipliedKey PreMultipliedKey { get; } = new();

    /// <summary>
    /// Whether the transition is reversed
    /// </summary>
    public bool Reverse { get; internal set; }

    /// <summary>
    /// Whether flip-flop is enabled
    /// </summary>
    public bool FlipFlop { get; internal set; }
}
