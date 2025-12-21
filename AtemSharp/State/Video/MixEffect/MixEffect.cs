using System.Diagnostics.CodeAnalysis;
using AtemSharp.State.Video.MixEffect.Transition;
using AtemSharp.Types;

namespace AtemSharp.State.Video.MixEffect;

/// <summary>
/// Mix effect block state containing program/preview inputs, transitions, and upstream keyers
/// </summary>
public class MixEffect
{
    public MixEffect()
    {
        UpstreamKeyers = new ItemCollection<byte, UpstreamKeyer.UpstreamKeyer>(id => new UpstreamKeyer.UpstreamKeyer
            {
                Id = id,
                MixEffectId = Id
            }
        );
    }

    /// <summary>
    /// Mix effect index (0-based)
    /// </summary>
    [ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
    public byte Id { get; internal init; }

    /// <summary>
    /// Currently selected program input source
    /// </summary>
    [ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
    public ushort ProgramInput { get; internal set; }

    /// <summary>
    /// Currently selected preview input source
    /// </summary>
    [ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
    public ushort PreviewInput { get; internal set; }

    /// <summary>
    /// Whether transition preview is enabled
    /// </summary>
    [ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
    public bool TransitionPreview { get; internal set; }

    /// <summary>
    /// Fade to black properties
    /// </summary>
    [ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
    public FadeToBlackProperties FadeToBlack { get; } = new();

    /// <summary>
    /// Current transition position
    /// </summary>
    [ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
    public TransitionPosition TransitionPosition { get; } = new();

    /// <summary>
    /// Transition properties for this mix effect
    /// </summary>
    [ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
    public TransitionProperties TransitionProperties { get; } = new();

    /// <summary>
    /// Transition settings for this mix effect
    /// </summary>
    [ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
    public TransitionSettings TransitionSettings { get; } = new();

    /// <summary>
    /// Upstream keyers for this mix effect
    /// </summary>
    [ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
    public ItemCollection<byte, UpstreamKeyer.UpstreamKeyer> UpstreamKeyers { get; }
}
