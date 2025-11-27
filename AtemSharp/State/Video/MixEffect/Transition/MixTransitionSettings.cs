using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Video.MixEffect.Transition;

/// <summary>
/// Settings for mix transitions
/// </summary>
[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class MixTransitionSettings
{
    /// <summary>
    /// Rate of the mix transition in frames (0-250)
    /// </summary>
    public byte Rate { get; internal set; }
}
