namespace AtemSharp.State;

/// <summary>
/// Settings for dip transitions
/// </summary>
public class DipTransitionSettings
{
    /// <summary>
    /// Rate of the dip transition in frames
    /// </summary>
    public int Rate { get; set; }

    /// <summary>
    /// Input source for the dip transition
    /// </summary>
    public int Input { get; set; }
}