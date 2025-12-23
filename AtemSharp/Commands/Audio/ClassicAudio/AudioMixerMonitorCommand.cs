using AtemSharp.State.Audio.ClassicAudio;

namespace AtemSharp.Commands.Audio.ClassicAudio;

/// <summary>
/// Command to update audio mixer monitor properties
/// </summary>
[Command("CAMm")]
[BufferSize(12)]
public partial class AudioMixerMonitorCommand(ClassicAudioState currentState) : SerializedCommand
{
    [SerializedField(1, 0)] private bool _enabled = currentState.Monitor.Enabled;

    [SerializedField(2, 1)] [CustomScaling($"{nameof(SerializationExtensions)}.{nameof(SerializationExtensions.DecibelToUInt16)}")]
    private double _gain = currentState.Monitor.Gain;

    [SerializedField(4, 2)] private bool _mute = currentState.Monitor.Mute;

    [SerializedField(5, 3)] private bool _solo = currentState.Monitor.Solo;

    [SerializedField(6, 4)] private ushort _soloSource = currentState.Monitor.SoloSource;

    [SerializedField(8, 5)] private bool _dim = currentState.Monitor.Dim;

    [SerializedField(10, 6)] [ScalingFactor(100.0)]
    private double _dimLevel = currentState.Monitor.DimLevel;
}
