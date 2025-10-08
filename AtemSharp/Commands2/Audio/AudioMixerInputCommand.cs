using AtemSharp.Enums;
using AtemSharp.Lib;

namespace AtemSharp.Commands2.Audio;

public class AudioMixerInputCommand(ushort index) : WritableCommand<AudioMixerInputCommand>
{
	public ushort Index => index;
	
	public override Stream Serialize(ProtocolVersion version)
	{
		var buffer = new MemoryStream();
		using var writer = new BinaryWriter(buffer);
		writer.Write(Flag);
		writer.WriteUInt16BE(index);
		writer.Write((byte)(MixOption ?? 0));
		writer.WriteUInt16BE(AtemUtil.DecibelToUInt16BE(Gain ?? 0.0));
		writer.WriteInt16BE(AtemUtil.BalanceToInt(Balance ?? 0.0));
		writer.Write(RcaToXlrEnabled ? (byte)1 : (byte)0);

		return buffer;
	}

	public AudioMixOption? MixOption { get; set; }

	public double? Gain { get; set; }

	public double? Balance { get; set; }

	public bool RcaToXlrEnabled { get; set; }

	protected override (byte MaskFlag, Func<AudioMixerInputCommand, object> Getter, Action<AudioMixerInputCommand, object> Setter)[]
		PropertyMap { get; } =
	[
		(1 << 0, cmd => cmd.MixOption, (cmd, val) => cmd.MixOption = (AudioMixOption)val),
		(1 << 1, cmd => cmd.Gain, (cmd, val) => cmd.Gain = (double)val),
		(1 << 2, cmd => cmd.Balance, (cmd, val) => cmd.Balance = (double)val),
		(1 << 3, cmd => cmd.RcaToXlrEnabled, (cmd, val) => cmd.RcaToXlrEnabled = (bool)val)
	];
}