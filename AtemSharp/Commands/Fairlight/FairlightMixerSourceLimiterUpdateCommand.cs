using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

[Command("AILP")]
public class FairlightMixerSourceLimiterUpdateCommand : FairlightMixerSourceUpdateCommandBase
{
    public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion version)
    {
        return new FairlightMixerSourceLimiterUpdateCommand
        {
            LimiterEnabled = rawCommand.ReadBoolean(16),
            Threshold = rawCommand.ReadInt32BigEndian(20) / 100.0,
            Attack = rawCommand.ReadInt32BigEndian(24) / 100.0,
            Hold = rawCommand.ReadInt32BigEndian(28) / 100.0,
            Release = rawCommand.ReadInt32BigEndian(32) / 100.0,
        }.DeserializeIds(rawCommand);
    }

    public double Release { get; set; }

    public double Hold { get; set; }

    public double Attack { get; set; }

    public double Threshold { get; set; }

    public bool LimiterEnabled { get; set; }

    protected override void ApplyToSource(Source source)
    {
        source.Dynamics.Limiter.Enabled = LimiterEnabled;
        source.Dynamics.Limiter.Threshold = Threshold;
        source.Dynamics.Limiter.Attack = Attack;
        source.Dynamics.Limiter.Hold = Hold;
        source.Dynamics.Limiter.Release = Release;
    }
}
