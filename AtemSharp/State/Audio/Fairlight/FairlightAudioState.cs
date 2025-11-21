namespace AtemSharp.State.Audio.Fairlight;

/// <summary>
/// Fairlight audio state for ATEM devices with Fairlight audio support
/// </summary>
public class FairlightAudioState : AudioState
{
    /// <summary>
    /// Fairlight audio inputs indexed by input number
    /// </summary>
    public Dictionary<int, FairlightAudioInput> Inputs { get; init; } = [];

    public MasterProperties Master { get; } = new();
    public MonitorProperties Monitor { get; } = new();
    public SoloProperties Solo { get; } = new();
}
