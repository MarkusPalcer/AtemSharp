namespace AtemSharp.State;

/// <summary>
/// Settings for wipe transitions
/// </summary>
public class WipeTransitionSettings
{
    /// <summary>
    /// Rate of the wipe transition in frames
    /// </summary>
    public int Rate { get; set; }

    /// <summary>
    /// Pattern for the wipe transition
    /// </summary>
    public int Pattern { get; set; }

    /// <summary>
    /// Width of the wipe border as percentage (0-100%)
    /// </summary>
    public double BorderWidth { get; set; }

    /// <summary>
    /// Input source for the wipe border
    /// </summary>
    public int BorderInput { get; set; }

    /// <summary>
    /// Symmetry setting for the wipe transition as percentage (0-100%)
    /// </summary>
    public double Symmetry { get; set; }

    /// <summary>
    /// Softness of the wipe border as percentage (0-100%)
    /// </summary>
    public double BorderSoftness { get; set; }

    /// <summary>
    /// X position for the wipe transition (0.0-1.0)
    /// </summary>
    public double XPosition { get; set; }

    /// <summary>
    /// Y position for the wipe transition (0.0-1.0)
    /// </summary>
    public double YPosition { get; set; }

    /// <summary>
    /// Whether the wipe direction is reversed
    /// </summary>
    public bool ReverseDirection { get; set; }

    /// <summary>
    /// Whether flip flop mode is enabled
    /// </summary>
    public bool FlipFlop { get; set; }
}