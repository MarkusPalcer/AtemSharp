using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

[Command("AILP")]
public class FairlightMixerSourceLimiterUpdateCommand : FairlightMixerSourceUpdateCommandBase
{
    public LimiterParameters Parameters { get; } = new();

    public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion version)
    {
        return new FairlightMixerSourceLimiterUpdateCommand
        {
            Parameters =
            {
                LimiterEnabled = rawCommand.ReadBoolean(16),
                Threshold = rawCommand.ReadInt32BigEndian(20) / 100.0,
                Attack = rawCommand.ReadInt32BigEndian(24) / 100.0,
                Hold = rawCommand.ReadInt32BigEndian(28) / 100.0,
                Release = rawCommand.ReadInt32BigEndian(32) / 100.0,
            }
        }.DeserializeIds(rawCommand);
    }


    protected override void ApplyToSource(Source source)
    {
        Parameters.ApplyTo(source.Dynamics.Limiter);
    }
}
