using System.Diagnostics.CodeAnalysis;
using AtemSharp.State.Audio;
using AtemSharp.State.Info;
using AtemSharp.State.Media;
using AtemSharp.State.Recording;
using AtemSharp.State.Settings;
using AtemSharp.State.Streaming;
using AtemSharp.State.Video;
using AtemSharp.Types;

namespace AtemSharp.State;

/// <summary>
/// Main ATEM state container
/// </summary>
public class AtemState
{
    public AtemState()
    {
        ColorGenerators = new ItemCollection<byte, ColorGeneratorState>(id => new ColorGeneratorState { Id = id });
    }

    /// <summary>
    /// Device information and capabilities
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public DeviceInfo Info { get; } = new();

    /// <summary>
    /// Audio state for classic ATEM devices
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public AudioState? Audio { get; internal set; }

    /// <summary>
    /// Display clock state and configuration
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public DisplayClock.DisplayClock DisplayClock { get; } = new();

    /// <summary>
    /// Video state including downstream keyers, mix effects, etc.
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public VideoState Video { get; } = new();

    /// <summary>
    /// Settings state including video mode and other device settings
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public SettingsState Settings { get; } = new();

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ItemCollection<byte, ColorGeneratorState> ColorGenerators { get; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public MediaState Media { get; } = new();

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public RecordingState Recording { get; } = new();

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public StreamingState Streaming { get; } = new();

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public TimeCode TimeCode { get; } = new();
}
