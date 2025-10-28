using AtemSharp.State.Audio;
using AtemSharp.State.Recording;

namespace AtemSharp.State;

/// <summary>
/// Main ATEM state container
/// </summary>
public class AtemState
{
    /// <summary>
    /// Device information and capabilities
    /// </summary>
    public DeviceInfo Info { get; internal set; } = new();

    /// <summary>
    /// Audio state for classic ATEM devices
    /// </summary>
    public AudioState? Audio { get; internal set; }

    /// <summary>
    /// Display clock state and configuration
    /// </summary>
    public DisplayClock? DisplayClock { get; internal set; }

    /// <summary>
    /// Video state including downstream keyers, mix effects, etc.
    /// </summary>
    public VideoState Video { get; internal set; } = new();

    /// <summary>
    /// Settings state including video mode and other device settings
    /// </summary>
    public SettingsState Settings { get; internal set; } = new();

    public Dictionary<int, ColorGeneratorState> ColorGenerators { get; } = new();

    public MediaState Media { get; } = new();

    public MacroState Macros { get; } = new();
    public RecordingState Recording { get; } = new();

    public RecordingDuration Duration { get; } = new();
}
