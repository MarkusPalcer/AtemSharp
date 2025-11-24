namespace AtemSharp.State;

/// <summary>
/// Settings for dip transitions
/// </summary>
public class DipTransitionSettings
{
    /// <summary>
    /// Rate of the dip transition in frames
    /// </summary>
    public byte Rate { get; internal set; }

    /// <summary>
    /// Input source for the dip transition
    /// </summary>
    public ushort Input { get; internal set; }
}
