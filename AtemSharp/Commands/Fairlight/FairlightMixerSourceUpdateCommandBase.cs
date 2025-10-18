using AtemSharp.Lib;
using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

public abstract class FairlightMixerSourceUpdateCommandBase : IDeserializedCommand
{
    public ushort InputId { get; protected set; }
    public long SourceId { get; protected set; }

    protected FairlightMixerSourceUpdateCommandBase() { }

    protected IDeserializedCommand DeserializeIds(ReadOnlySpan<byte> rawCommand)
    {
        InputId = rawCommand.ReadUInt16BigEndian(0);
        SourceId = rawCommand.ReadInt64BigEndian(8);
        return this;
    }

    public void ApplyToState(AtemState state)
    {
        var audio = state.GetFairlight();

        if (!audio.Inputs.TryGetValue(InputId, out var input))
        {
            throw new IndexOutOfRangeException($"Input ID {InputId} does not exist");
        }

        var source = input.Sources.GetOrCreate(SourceId);
        source.Id = SourceId;
        source.InputId = InputId;

        ApplyToSource(source);
    }

    protected abstract void ApplyToSource(Source source);
}
