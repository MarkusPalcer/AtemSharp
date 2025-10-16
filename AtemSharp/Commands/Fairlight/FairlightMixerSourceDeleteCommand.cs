using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

[Command("FASD")]
public class FairlightMixerSourceDeleteCommand : IDeserializedCommand
{
    public ushort InputId { get; set; }
    public long SourceId { get; set; }

    public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion version)
    {
        return new FairlightMixerSourceDeleteCommand
        {
            InputId = rawCommand.ReadUInt16BigEndian(0),
            SourceId = rawCommand.ReadInt64BigEndian(8)
        };
    }

    public void ApplyToState(AtemState state)
    {
        if (state.Audio is not FairlightAudioState audio)
        {
            throw new InvalidOperationException("State is not Fairlight audio");
        }

        if (!audio.Inputs.TryGetValue(InputId, out var input))
        {
            throw new IndexOutOfRangeException($"Input ID {InputId} does not exist");
        }

        input.Sources.Remove(SourceId);
    }
}
