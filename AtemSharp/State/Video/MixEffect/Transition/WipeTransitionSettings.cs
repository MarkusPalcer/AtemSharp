namespace AtemSharp.State.Video.MixEffect.Transition;

/// <summary>
/// Settings for wipe transitions
/// </summary>
public class WipeTransitionSettings
{
    /// <summary>
    /// Rate of the wipe transition in frames
    /// </summary>
    public byte Rate { get; internal set; }

    /// <summary>
    /// Pattern for the wipe transition
    /// </summary>
    public byte Pattern { get; internal set; }

    /// <summary>
    /// Width of the wipe border as percentage (0-100%)
    /// </summary>
    public double BorderWidth { get; internal set; }

    /// <summary>
    /// Input source for the wipe border
    /// </summary>
    public ushort BorderInput { get; internal set; }

    /// <summary>
    /// Symmetry setting for the wipe transition as percentage (0-100%)
    /// </summary>
    public double Symmetry { get; internal set; }

    /// <summary>
    /// Softness of the wipe border as percentage (0-100%)
    /// </summary>
    public double BorderSoftness { get; internal set; }

    /// <summary>
    /// X position for the wipe transition (0.0-1.0)
    /// </summary>
    public double XPosition { get; internal set; }

    /// <summary>
    /// Y position for the wipe transition (0.0-1.0)
    /// </summary>
    public double YPosition { get; internal set; }

    /// <summary>
    /// Whether the wipe direction is reversed
    /// </summary>
    public bool ReverseDirection { get; internal set; }

    /// <summary>
    /// Whether flip flop mode is enabled
    /// </summary>
    public bool FlipFlop { get; internal set; }
}
