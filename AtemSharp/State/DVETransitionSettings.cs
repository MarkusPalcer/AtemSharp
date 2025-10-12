using AtemSharp.Enums;

namespace AtemSharp.State;

/// <summary>
/// DVE transition settings for a mix effect
/// </summary>
// ReSharper disable once InconsistentNaming Domain Specific Acronym
public class DVETransitionSettings
{
    /// <summary>
    /// Transition rate in frames
    /// </summary>
    public int Rate { get; set; }

    /// <summary>
    /// Logo/key transition rate in frames
    /// </summary>
    public int LogoRate { get; set; }

    /// <summary>
    /// DVE effect style
    /// </summary>
    public DVEEffect Style { get; set; }

    /// <summary>
    /// Fill source input number
    /// </summary>
    public int FillSource { get; set; }

    /// <summary>
    /// Key source input number
    /// </summary>
    public int KeySource { get; set; }

    /// <summary>
    /// Whether the key is enabled
    /// </summary>
    public bool EnableKey { get; set; }

    /// <summary>
    /// Whether the key is pre-multiplied
    /// </summary>
    public bool PreMultiplied { get; set; }

    /// <summary>
    /// Key clip value (0.0 to 100.0)
    /// </summary>
    public double Clip { get; set; }

    /// <summary>
    /// Key gain value (0.0 to 100.0)
    /// </summary>
    public double Gain { get; set; }

    /// <summary>
    /// Whether the key is inverted
    /// </summary>
    public bool InvertKey { get; set; }

    /// <summary>
    /// Whether the transition is reversed
    /// </summary>
    public bool Reverse { get; set; }

    /// <summary>
    /// Whether flip-flop is enabled
    /// </summary>
    public bool FlipFlop { get; set; }
}