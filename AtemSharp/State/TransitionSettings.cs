namespace AtemSharp.State;

/// <summary>
/// Container for all transition settings for a mix effect
/// </summary>
public class TransitionSettings
{
    /// <summary>
    /// Mix transition settings
    /// </summary>
    public MixTransitionSettings Mix { get; } = new();

    /// <summary>
    /// Dip transition settings
    /// </summary>
    public DipTransitionSettings Dip { get; } = new();

    /// <summary>
    /// Wipe transition settings
    /// </summary>
    public WipeTransitionSettings Wipe { get; } = new();

    /// <summary>
    /// DigitalVideoEffect transition settings
    /// </summary>
    public DigitalVideoEffectTransitionSettings DigitalVideoEffect { get; } = new();

    /// <summary>
    /// Stinger transition settings
    /// </summary>
    public StingerTransitionSettings Stinger { get; } = new();
}
