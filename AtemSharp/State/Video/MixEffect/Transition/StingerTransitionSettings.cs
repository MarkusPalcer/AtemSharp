using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Video.MixEffect.Transition;

/// <summary>
/// Settings for stinger transition effects
/// </summary>
[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class StingerTransitionSettings
{
    /// <summary>
    /// Source for the stinger transition
    /// </summary>
    public byte Source { get; internal set; }

    /// <summary>
    /// Whether the key is pre-multiplied
    /// </summary>
    public bool PreMultipliedKey { get; internal set; }

    /// <summary>
    /// Clip value for the stinger transition
    /// </summary>
    public double Clip { get; internal set; }

    /// <summary>
    /// Gain value for the stinger transition (0-100%)
    /// </summary>
    public double Gain { get; internal set; }

    /// <summary>
    /// Whether the stinger transition is inverted
    /// </summary>
    public bool Invert { get; internal set; }

    /// <summary>
    /// Preroll value for the stinger transition
    /// </summary>
    public ushort Preroll { get; internal set; }

    /// <summary>
    /// Clip duration for the stinger transition
    /// </summary>
    public ushort ClipDuration { get; internal set; }

    /// <summary>
    /// Trigger point for the stinger transition
    /// </summary>
    public ushort TriggerPoint { get; internal set; }

    /// <summary>
    /// Mix rate for the stinger transition
    /// </summary>
    public ushort MixRate { get; internal set; }
}
