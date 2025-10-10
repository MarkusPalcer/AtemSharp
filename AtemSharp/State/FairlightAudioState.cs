namespace AtemSharp.State;

/// <summary>
/// Fairlight audio state for ATEM devices with Fairlight audio support
/// </summary>
public class FairlightAudioState
{
    /// <summary>
    /// Fairlight audio inputs indexed by input number
    /// </summary>
    public Dictionary<int, FairlightAudioInput> Inputs { get; set; } = new();
}