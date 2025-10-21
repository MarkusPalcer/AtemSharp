using AtemSharp.Enums;
using AtemSharp.Helpers;
using AtemSharp.Lib;
using AtemSharp.State;
using AtemSharp.State.Audio.ClassicAudio;

namespace AtemSharp.Commands.Audio;

/// <summary>
/// Update command for audio mixer monitor properties
/// </summary>
[Command("AMmO")]
public partial class AudioMixerMonitorUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Whether the monitor is enabled
    /// </summary>
    [DeserializedField(0)]
    private bool _enabled;

    /// <summary>
    /// Gain in decibel, -Infinity to +6dB
    /// </summary>
    public double Gain { get; internal set; }

    /// <summary>
    /// Whether the monitor is muted
    /// </summary>
    [DeserializedField(4)]
    private bool _mute;

    /// <summary>
    /// Whether solo is enabled
    /// </summary>
    [DeserializedField(5)]
    private bool _solo;

    /// <summary>
    /// Solo source identifier
    /// </summary>
    [DeserializedField(6)]
    private ushort _soloSource;

    /// <summary>
    /// Whether dim is enabled
    /// </summary>
    [DeserializedField(8)]
    private bool _dim;

    /// <summary>
    /// Dim level as percentage (0.0 to 1.0)
    /// </summary>
    [DeserializedField(10)]
    [ScalingFactor(100.0)]
    private double _dimLevel;



    private void DeserializeInternal(ReadOnlySpan<byte> rawCommand, ProtocolVersion _)
    {
        Gain = rawCommand.ReadUInt16BigEndian(2).UInt16ToDecibel();
    }

	/// <inheritdoc />
	public void ApplyToState(AtemState state)
	{
        var audio = state.GetClassicAudio();
		audio.Monitor ??= new ClassicAudioMonitorChannel();
		audio.Monitor.Enabled = Enabled;
		audio.Monitor.Gain = Gain;
		audio.Monitor.Mute = Mute;
		audio.Monitor.Solo = Solo;
		audio.Monitor.SoloSource = SoloSource;
		audio.Monitor.Dim = Dim;
		audio.Monitor.DimLevel = DimLevel;
	}
}
