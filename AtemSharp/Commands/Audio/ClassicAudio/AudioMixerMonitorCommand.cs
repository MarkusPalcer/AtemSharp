using AtemSharp.State.Audio.ClassicAudio;

namespace AtemSharp.Commands.Audio.ClassicAudio;

/// <summary>
/// Command to update audio mixer monitor properties
/// </summary>
[Command("CAMm")]
[BufferSize(12)]
public partial class AudioMixerMonitorCommand(ClassicAudioState currentState) : SerializedCommand
{
    /// <summary>
    /// Whether the monitor is enabled
    /// </summary>
    [SerializedField(1, 0)] private bool _enabled = currentState.Monitor.Enabled;

    /// <summary>
    /// Gain in decibel
    /// </summary>
    [SerializedField(2, 1)] [CustomScaling($"{nameof(SerializationExtensions)}.{nameof(SerializationExtensions.DecibelToUInt16)}")]
    private double _gain = currentState.Monitor.Gain;

    /// <summary>
    /// Whether the monitor is muted
    /// </summary>
    [SerializedField(4, 2)] private bool _mute = currentState.Monitor.Mute;

    /// <summary>
    /// Whether solo is enabled
    /// </summary>
    [SerializedField(5, 3)] private bool _solo = currentState.Monitor.Solo;

    /// <summary>
    /// Solo source identifier
    /// </summary>
    [SerializedField(6, 4)] private ushort _soloSource = currentState.Monitor.SoloSource;

    /// <summary>
    /// Whether dim is enabled
    /// </summary>
    [SerializedField(8, 5)] private bool _dim = currentState.Monitor.Dim;

    /// <summary>
    /// Dim level as percentage (0.0 to 1.0)
    /// </summary>
    [SerializedField(10, 6)] [ScalingFactor(100.0)]
    private double _dimLevel = currentState.Monitor.DimLevel;
}
