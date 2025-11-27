using System.Diagnostics.CodeAnalysis;
using AtemSharp.State.Video.MixEffect.Transition;

namespace AtemSharp.State.Video.MixEffect;

/// <summary>
/// Mix effect block state containing program/preview inputs, transitions, and upstream keyers
/// </summary>
[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class MixEffect : ArrayItem
{
    internal override void SetId(int id) => Id = (byte)id;

    /// <summary>
    /// Mix effect index (0-based)
    /// </summary>
    public byte Id { get; internal set; }

    /// <summary>
    /// Currently selected program input source
    /// </summary>
    public ushort ProgramInput { get; internal set; }

    /// <summary>
    /// Currently selected preview input source
    /// </summary>
    public ushort PreviewInput { get; internal set; }

    /// <summary>
    /// Whether transition preview is enabled
    /// </summary>
    public bool TransitionPreview { get; internal set; }

    /// <summary>
    /// Fade to black properties
    /// </summary>
    public FadeToBlackProperties FadeToBlack { get; } = new();

    /// <summary>
    /// Current transition position
    /// </summary>
    public TransitionPosition TransitionPosition { get; } = new();

    /// <summary>
    /// Transition properties for this mix effect
    /// </summary>
    public TransitionProperties TransitionProperties { get; } = new();

    /// <summary>
    /// Transition settings for this mix effect
    /// </summary>
    public TransitionSettings TransitionSettings { get; } = new();

    /// <summary>
    /// Upstream keyers for this mix effect
    /// </summary>
    public Dictionary<int, UpstreamKeyer.UpstreamKeyer> UpstreamKeyers { get; } = new();
}
