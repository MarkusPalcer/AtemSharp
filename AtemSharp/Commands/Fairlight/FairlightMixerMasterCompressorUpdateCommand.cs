using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Fairlight;

[Command("MOCP")]
public class FairlightMixerMasterCompressorUpdateCommand : IDeserializedCommand
{
    public CompressorParameters Parameters { get; } = new();

    public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion version)
    {
        return new FairlightMixerMasterCompressorUpdateCommand
        {
            Parameters = {
                CompressorEnabled = rawCommand.ReadBoolean(0),
                Threshold = rawCommand.ReadInt32BigEndian(4) / 100.0,
                Ratio = rawCommand.ReadInt16BigEndian(8) / 100.0,
                Attack = rawCommand.ReadInt32BigEndian(12) / 100.0,
                Hold = rawCommand.ReadInt32BigEndian(16) / 100.0,
                Release = rawCommand.ReadInt32BigEndian(20) / 100.0,
            }
        };
    }
    public void ApplyToState(AtemState state)
    {
        Parameters.ApplyTo(state.GetFairlight().Master.Dynamics.Compressor);
    }
}
