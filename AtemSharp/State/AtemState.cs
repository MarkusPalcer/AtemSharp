using AtemSharp.State.Audio;
using AtemSharp.State.Info;
using AtemSharp.State.Macro;
using AtemSharp.State.Media;
using AtemSharp.State.Recording;
using AtemSharp.State.Settings;
using AtemSharp.State.Streaming;
using AtemSharp.State.Video;

namespace AtemSharp.State;

/// <summary>
/// Main ATEM state container
/// </summary>
public class AtemState
{
    /// <summary>
    /// Device information and capabilities
    /// </summary>
    public DeviceInfo Info { get; } = new();

    /// <summary>
    /// Audio state for classic ATEM devices
    /// </summary>
    public AudioState? Audio { get; internal set; }

    /// <summary>
    /// Display clock state and configuration
    /// </summary>
    public DisplayClock.DisplayClock DisplayClock { get; } = new();

    /// <summary>
    /// Video state including downstream keyers, mix effects, etc.
    /// </summary>
    public VideoState Video { get; } = new();

    /// <summary>
    /// Settings state including video mode and other device settings
    /// </summary>
    public SettingsState Settings { get; } = new();

    public List<ColorGeneratorState> ColorGenerators { get; } = [];

    public MediaState Media { get; } = new();

    public MacroState Macros { get; } = new();
    public RecordingState Recording { get; } = new();

    public StreamingState Streaming { get; } = new();

    public TimeCode TimeCode { get; } = new();
}
