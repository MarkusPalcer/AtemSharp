namespace AtemSharp.State;

/// <summary>
/// Main ATEM state container
/// </summary>
public class AtemState
{
    public DeviceInfo Info { get; set; } = new();
    public VideoState Video { get; } = new();
    public SettingsState Settings { get; set; } = new();
    public Dictionary<int, InputChannelState> Inputs { get; set; } = new();
    public AudioState? Audio { get; set; } 

}