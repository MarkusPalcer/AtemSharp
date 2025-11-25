using AtemSharp.Enums;

namespace AtemSharp.State;

/// <summary>
/// Settings state container for ATEM devices
/// </summary>
public class SettingsState
{
    /// <summary>
    /// Current video mode of the device
    /// </summary>
    public VideoMode VideoMode { get; internal set; }

    /// <summary>
    /// Current time mode of the device
    /// </summary>
    public TimeMode TimeMode { get; internal set; }

    /// <summary>
    /// MultiViewer settings indexed by MultiViewer ID using sparse Dictionary for efficient memory usage
    /// </summary>
    public List<MultiViewer> MultiViewers { get; } = [];

    public MediaPoolSettings MediaPool { get; } = new();
}
