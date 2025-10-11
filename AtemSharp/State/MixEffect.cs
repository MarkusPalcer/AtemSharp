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
    /// Transition properties
    /// </summary>
    public TransitionProperties TransitionProperties { get; set; } = new();

    /// <summary>
    /// Transition settings
    /// </summary>
    public TransitionSettings TransitionSettings { get; set; } = new();

    /// <summary>
    /// Upstream keyers for this mix effect
    /// </summary>
    public UpstreamKeyer?[] UpstreamKeyers { get; set; } = [];
}

/// <summary>
/// Fade to black properties for a mix effect
/// </summary>
public class FadeToBlackProperties
{
    /// <summary>
    /// Whether the output is fully black
    /// </summary>
    public bool IsFullyBlack { get; set; }

    /// <summary>
    /// Whether a fade to black transition is in progress
    /// </summary>
    public bool InTransition { get; set; }

    /// <summary>
    /// Number of frames remaining in the transition
    /// </summary>
    public int RemainingFrames { get; set; }

    /// <summary>
    /// Fade to black rate (frames)
    /// </summary>
    public int Rate { get; set; }
}

/// <summary>
/// Current transition position information
/// </summary>
public class TransitionPosition
{
    /// <summary>
    /// Whether a transition is currently in progress
    /// </summary>
    public bool InTransition { get; set; }

    /// <summary>
    /// Number of frames remaining in the transition
    /// </summary>
    public int RemainingFrames { get; set; }

    /// <summary>
    /// Current position of the transition handle (0.0 to 1.0)
    /// </summary>
    public double HandlePosition { get; set; }
}

/// <summary>
/// Transition properties
/// </summary>
public class TransitionProperties
{
    // TODO: Implement transition properties based on TypeScript implementation
    // This will be filled in when implementing transition commands
}

/// <summary>
/// Transition settings
/// </summary>
public class TransitionSettings
{
    // TODO: Implement transition settings based on TypeScript implementation
    // This will be filled in when implementing transition commands
}

/// <summary>
/// Upstream keyer (placeholder for future implementation)
/// </summary>
public class UpstreamKeyer
{
    // TODO: Implement upstream keyer based on TypeScript implementation
    // This will be filled in when implementing upstream keyer commands
}