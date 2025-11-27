namespace AtemSharp.State.Video.MixEffect.Transition;

/// <summary>
/// Settings for mix transitions
/// </summary>
public class MixTransitionSettings
{
    /// <summary>
    /// Rate of the mix transition in frames (0-250)
    /// </summary>
    public byte Rate { get; internal set; }
}
