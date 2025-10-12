namespace AtemSharp.State;

/// <summary>
/// Container for all transition settings for a mix effect
/// </summary>
public class TransitionSettings
{
    /// <summary>
    /// Mix transition settings
    /// </summary>
    public MixTransitionSettings? Mix { get; set; }

    /// <summary>
    /// Dip transition settings
    /// </summary>
    public DipTransitionSettings? Dip { get; set; }

    /// <summary>
    /// Wipe transition settings
    /// </summary>
    public WipeTransitionSettings? Wipe { get; set; }

    /// <summary>
    /// DVE transition settings
    /// </summary>
    // ReSharper disable once InconsistentNaming Domain Specific Acronym
    public DVETransitionSettings? DVE { get; set; }

    // TODO: Add other transition settings as they are implemented:
    // public StingerTransitionSettings? Stinger { get; set; }
}