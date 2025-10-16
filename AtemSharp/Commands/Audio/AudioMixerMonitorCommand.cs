using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Audio;

/// <summary>
/// Command to update audio mixer monitor properties
/// </summary>
[Command("CAMm")]
public class AudioMixerMonitorCommand : SerializedCommand
{
	private bool _enabled;
	private double _gain;
	private bool _mute;
	private bool _solo;
	private int _soloSource;
	private bool _dim;
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

	/// <summary>
	/// Whether the monitor is enabled
	/// </summary>
	public bool Enabled
	{
		get => _enabled;
		set
		{
			_enabled = value;
			Flag |= 1 << 0;
		}
	}

	/// <summary>
	/// Gain in decibel, -Infinity to +6dB
	/// </summary>
	public double Gain
	{
		get => _gain;
		set
		{
			_gain = value;
			Flag |= 1 << 1;
		}
	}

	/// <summary>
	/// Whether the monitor is muted
	/// </summary>
	public bool Mute
	{
		get => _mute;
		set
		{
			_mute = value;
			Flag |= 1 << 2;
		}
	}

	/// <summary>
	/// Whether solo is enabled
	/// </summary>
	public bool Solo
	{
		get => _solo;
		set
		{
			_solo = value;
			Flag |= 1 << 3;
		}
	}

	/// <summary>
	/// Solo source identifier
	/// </summary>
	public int SoloSource
	{
		get => _soloSource;
		set
		{
			_soloSource = value;
			Flag |= 1 << 4;
		}
	}

	/// <summary>
	/// Whether dim is enabled
	/// </summary>
	public bool Dim
	{
		get => _dim;
		set
		{
			_dim = value;
			Flag |= 1 << 5;
		}
	}

	/// <summary>
	/// Dim level as percentage (0.0 to 1.0)
	/// </summary>
	public double DimLevel
	{
		get => _dimLevel;
		set
		{
			_dimLevel = value;
			Flag |= 1 << 6;
		}
	}

	/// <inheritdoc />
	public override byte[] Serialize(ProtocolVersion version)
	{
		using var memoryStream = new MemoryStream(12);
		using var writer = new BinaryWriter(memoryStream);

		writer.Write((byte)Flag);
		writer.WriteBoolean(Enabled);
		writer.WriteUInt16BigEndian(Gain.DecibelToUInt16());
		writer.WriteBoolean(Mute);
		writer.WriteBoolean(Solo);
		writer.WriteUInt16BigEndian((ushort)SoloSource);
		writer.WriteBoolean(Dim);
		writer.Pad(1);
		writer.WriteUInt16BigEndian((ushort)Math.Round(DimLevel * 100));

		return memoryStream.ToArray();
	}
}
