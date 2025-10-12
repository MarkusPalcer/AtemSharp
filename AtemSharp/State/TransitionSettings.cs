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

    // TODO: Add other transition settings as they are implemented:
    // public DipTransitionSettings? Dip { get; set; }
    // public DVETransitionSettings? DVE { get; set; }
    // public StingerTransitionSettings? Stinger { get; set; }
    // public WipeTransitionSettings? Wipe { get; set; }
}