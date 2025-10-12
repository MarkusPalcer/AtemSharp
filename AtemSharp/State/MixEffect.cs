namespace AtemSharp.State;

/// <summary>
/// Mix effect block state containing program/preview inputs, transitions, and upstream keyers
/// </summary>
public class MixEffect
{
    /// <summary>
    /// Mix effect index (0-based)
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// Currently selected program input source
    /// </summary>
    public int ProgramInput { get; set; }

    /// <summary>
    /// Currently selected preview input source
    /// </summary>
    public int PreviewInput { get; set; }

    /// <summary>
    /// Whether transition preview is enabled
    /// </summary>
    public bool TransitionPreview { get; set; }

    /// <summary>
    /// Fade to black properties
    /// </summary>
    public FadeToBlackProperties? FadeToBlack { get; set; }

    /// <summary>
    /// Current transition position
    /// </summary>
    public TransitionPosition TransitionPosition { get; set; } = new();

    /// <summary>
    /// Transition properties for this mix effect
    /// </summary>
    public TransitionProperties? TransitionProperties { get; set; }

    /// <summary>
    /// Transition settings for this mix effect
    /// </summary>
    public TransitionSettings? TransitionSettings { get; set; }
}