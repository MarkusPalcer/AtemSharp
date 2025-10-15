namespace AtemSharp.State;

/// <summary>
/// Fairlight audio state for ATEM devices with Fairlight audio support
/// </summary>
public class FairlightAudioState : AudioState
{
    /// <summary>
    /// Fairlight audio inputs indexed by input number
    /// </summary>
    public Dictionary<int, FairlightAudioInput> Inputs { get; init; } = new();

    public MasterProperties Master { get; } = new();
}

public class MasterProperties
{
    /// <summary>
    /// Whether audio follows video during crossfade transition
    /// </summary>
    public bool AudioFollowsVideo { get; set; }
}
