namespace AtemSharp.State;

/// <summary>
/// Mix effect block state containing program/preview inputs, transitions, and upstream keyers
/// </summary>
public class MixEffect
{
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
    public FadeToBlackProperties? FadeToBlack { get; internal set; }

    /// <summary>
    /// Current transition position
    /// </summary>
    public TransitionPosition TransitionPosition { get; internal set; } = new();

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
    // TODO: Convert to Array and create once number of UpstreamKeyers is sent
    public Dictionary<int, UpstreamKeyer> UpstreamKeyers { get; internal set; } = new();
}
