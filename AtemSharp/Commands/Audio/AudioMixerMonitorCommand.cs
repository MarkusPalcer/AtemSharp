using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Audio;

/// <summary>
/// Command to update audio mixer monitor properties
/// </summary>
[Command("CAMm")]
[BufferSize(12)]
public partial class AudioMixerMonitorCommand : SerializedCommand
{
    /// <summary>
    /// Whether the monitor is enabled
    /// </summary>
	[SerializedField(1, 0)]
    private bool _enabled;

    /// <summary>
    /// Gain in decibel
    /// </summary>
	[SerializedField(2, 1)]
    [CustomScaling($"{nameof(AtemUtil)}.{nameof(AtemUtil.DecibelToUInt16)}")]
    private double _gain;

    /// <summary>
    /// Whether the monitor is muted
    /// </summary>
	[SerializedField(4,2)]
    private bool _mute;

    /// <summary>
    /// Whether solo is enabled
    /// </summary>
	[SerializedField(5, 3)]
    private bool _solo;

    /// <summary>
    /// Solo source identifier
    /// </summary>
	[SerializedField(6, 4)]
    private ushort _soloSource;

    /// <summary>
    /// Whether dim is enabled
    /// </summary>
	[SerializedField(8, 5)]
    private bool _dim;

    /// <summary>
    /// Dim level as percentage (0.0 to 1.0)
    /// </summary>
	[SerializedField(10, 6)]
    [ScalingFactor(100.0)]
    private double _dimLevel;

	public AudioMixerMonitorCommand(AtemState currentState)
	{
        var audio = currentState.GetClassicAudio();

        if (audio.Monitor is null)
        {
            throw new InvalidOperationException("Master audio channel is not available (yet)");
        }

		_enabled = audio.Monitor.Enabled;
		_gain = audio.Monitor.Gain;
		_mute = audio.Monitor.Mute;
		_solo = audio.Monitor.Solo;
		_soloSource = audio.Monitor.SoloSource;
		_dim = audio.Monitor.Dim;
		_dimLevel = audio.Monitor.DimLevel;
	}
}
