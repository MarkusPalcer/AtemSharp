using System.Diagnostics.CodeAnalysis;
using AtemSharp.Types;

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

    public PreMultipliedKey PreMultipliedKey { get; } = new();

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
