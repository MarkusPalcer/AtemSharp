using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Video.MixEffect;

/// <summary>
/// Fade to black properties for a mix effect
/// </summary>
[ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
[DebuggerDisplay("{" + nameof(ToString) + ",nq}")]
public class FadeToBlackProperties
{
    /// <summary>
    /// Whether the output is fully black
    /// </summary>
    public bool IsFullyBlack { get; internal set; }

    /// <summary>
    /// Whether a fade to black transition is in progress
    /// </summary>
    public bool InTransition { get; internal set; }

    /// <summary>
    /// Number of frames remaining in the transition
    /// </summary>
    public int RemainingFrames { get; internal set; }

    /// <summary>
    /// Fade to black rate (frames)
    /// </summary>
    public byte Rate { get; internal set; }
}
