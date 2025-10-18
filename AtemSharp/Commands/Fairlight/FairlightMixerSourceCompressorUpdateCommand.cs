using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

[Command("AICP")]
public class FairlightMixerSourceCompressorUpdateCommand : FairlightMixerSourceUpdateCommandBase
{
    public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion version)
    {
        return new FairlightMixerSourceCompressorUpdateCommand
        {
            CompressorEnabled = rawCommand.ReadBoolean(16),
            Threshold = rawCommand.ReadInt32BigEndian(20) / 100.0,
            Ratio = rawCommand.ReadInt16BigEndian(24) / 100.0,
            Attack = rawCommand.ReadInt32BigEndian(28) / 100.0,
            Hold = rawCommand.ReadInt32BigEndian(32) / 100.0,
            Release = rawCommand.ReadInt32BigEndian(36) / 100.0,
        }.DeserializeIds(rawCommand);
    }

    public double Release { get; set; }

    public double Hold { get; set; }

    public double Attack { get; set; }

    public double Ratio { get; set; }

    public double Threshold { get; set; }

    public bool CompressorEnabled { get; set; }


    protected override void ApplyToSource(Source source)
    {
        source.Dynamics.Compressor.Enabled = CompressorEnabled;
        source.Dynamics.Compressor.Threshold = Threshold;
        source.Dynamics.Compressor.Ratio = Ratio;
        source.Dynamics.Compressor.Attack = Attack;
        source.Dynamics.Compressor.Hold = Hold;
        source.Dynamics.Compressor.Release = Release;
    }
}
