namespace AtemSharp.State;

/// <summary>
/// Settings for stinger transition effects
/// </summary>
public class StingerTransitionSettings
{
    /// <summary>
    /// Source for the stinger transition
    /// </summary>
    public byte Source { get; set; }

    /// <summary>
    /// Whether the key is pre-multiplied
    /// </summary>
    public bool PreMultipliedKey { get; set; }

    /// <summary>
    /// Clip value for the stinger transition
    /// </summary>
    public double Clip { get; set; }

    /// <summary>
    /// Gain value for the stinger transition (0-100%)
    /// </summary>
    public double Gain { get; set; }

    /// <summary>
    /// Whether the stinger transition is inverted
    /// </summary>
    public bool Invert { get; set; }

    /// <summary>
    /// Preroll value for the stinger transition
    /// </summary>
    public ushort Preroll { get; set; }

    /// <summary>
    /// Clip duration for the stinger transition
    /// </summary>
    public ushort ClipDuration { get; set; }

    /// <summary>
    /// Trigger point for the stinger transition
    /// </summary>
    public ushort TriggerPoint { get; set; }

    /// <summary>
    /// Mix rate for the stinger transition
    /// </summary>
    public ushort MixRate { get; set; }
}
