using AtemSharp.State.Audio;
using AtemSharp.State.Info;

namespace AtemSharp.State;

/// <summary>
/// Main ATEM state container
/// </summary>
public class AtemState
{
    /// <summary>
    /// Device information and capabilities
    /// </summary>
    public DeviceInfo Info { get; set; } = new();

    /// <summary>
    /// Audio state for classic ATEM devices
    /// </summary>
    public AudioState? Audio { get; set; }

    /// <summary>
    /// Display clock state and configuration
    /// </summary>
    public DisplayClock? DisplayClock { get; set; }

    /// <summary>
    /// Video state including downstream keyers, mix effects, etc.
    /// </summary>
    public VideoState Video { get; set; } = new();

    /// <summary>
    /// Settings state including video mode and other device settings
    /// </summary>
    public SettingsState Settings { get; set; } = new();

    public Dictionary<int, ColorGeneratorState> ColorGenerators { get; } = new();

    public MediaState Media { get; } = new();

    public MacroState Macros { get; } = new();
    public RecordingState Recording { get; } = new();
}
