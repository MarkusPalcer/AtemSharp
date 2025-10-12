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
    public VideoMode VideoMode { get; set; }
}