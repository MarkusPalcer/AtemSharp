using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Audio.Fairlight.AudioRouting;

[Command("ARSP")]
internal partial class AudioRoutingSourceUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private uint _id;

    [DeserializedField(4)]
    private ushort _externalPortType;

    [DeserializedField(6)]
    private ushort _internalPortType;

    [CustomDeserialization]
    private uint _audioSourceId;

    [CustomDeserialization]
    private AudioChannelPair _audioChannelPair;

    // Stryker disable once string : initialization is always overriden by deserialization
    [CustomDeserialization]
    private string _name = string.Empty;

    private void DeserializeInternal(ReadOnlySpan<byte> rawCommand)
    {
        // Stryker disable once bitwise : >> and >>> do the same for unsigned types
        _audioSourceId = _id >> 16;
        _audioChannelPair = (AudioChannelPair)(ushort)(_id & 0xFFFF);

        _name = rawCommand.ReadString(8, 64);
    }

    public void ApplyToState(AtemState state)
    {
        var output = state.GetFairlight().AudioRouting.Sources.GetOrCreate(_id);

        output.Id = _audioSourceId;
        output.ChannelPair = _audioChannelPair;
        output.InternalPortType = _internalPortType;
        output.ExternalPortType = _externalPortType;
        output.Name = _name;
    }
}
