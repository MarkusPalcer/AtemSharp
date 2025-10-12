namespace AtemSharp.Enums;

/// <summary>
/// Transition style for mix effect block transitions
/// </summary>
public enum TransitionStyle : byte
{
    /// <summary>
    /// Mix transition (dissolve)
    /// </summary>
    Mix = 0x00,
    
    /// <summary>
    /// Dip transition (fade to color/black then fade in)
    /// </summary>
    Dip = 0x01,
    
    /// <summary>
    /// Wipe transition (geometric patterns)
    /// </summary>
    Wipe = 0x02,
    
    /// <summary>
    /// DVE transition (digital video effects)
    /// </summary>
    // ReSharper disable once InconsistentNaming Domain specific acronym
    DVE = 0x03,
    
    /// <summary>
    /// Stinger transition (video overlay)
    /// </summary>
    Sting = 0x04
}