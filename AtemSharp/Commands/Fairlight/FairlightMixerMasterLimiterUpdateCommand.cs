using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

[Command("AMLP")]
public class FairlightMixerMasterLimiterUpdateCommand : IDeserializedCommand
{
    internal LimiterParameters Parameters { get; } = new();

    public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion version)
    {
        return new FairlightMixerMasterLimiterUpdateCommand
        {
            Parameters =
            {
                LimiterEnabled = rawCommand.ReadBoolean(0),
                Threshold = rawCommand.ReadInt32BigEndian(4) / 100.0,
                Attack = rawCommand.ReadInt32BigEndian(8) / 100.0,
                Hold = rawCommand.ReadInt32BigEndian(12) / 100.0,
                Release = rawCommand.ReadInt32BigEndian(16) / 100.0,
            }
        };
    }

    public void ApplyToState(AtemState state)
    {
        Parameters.ApplyTo(state.GetFairlight().Master.Dynamics.Limiter);
    }
}
