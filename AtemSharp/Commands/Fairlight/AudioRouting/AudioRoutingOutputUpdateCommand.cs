using AtemSharp.Attributes;
using AtemSharp.Enums;
using AtemSharp.Helpers;
using AtemSharp.Lib;
using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight.AudioRouting;

[Command("AROP")]
public partial class AudioRoutingOutputUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private uint _id;

    [DeserializedField(4)]
    private uint _sourceId;

    [DeserializedField(8)]
    private ushort _externalPortType;

    [DeserializedField(10)]
    private ushort _internalPortType;

    [DeserializedField(0)]
    [CustomDeserialization]
    private string _name = string.Empty;

    [DeserializedField(0)]
    [CustomDeserialization]
    private uint _audioOutputId;

    [DeserializedField(0)]
    [CustomDeserialization]
    private AudioChannelPair _audioChannelPair;

    private void DeserializeInternal(ReadOnlySpan<byte> rawCommand, ProtocolVersion _)
    {
        _audioOutputId = _id >> 16;
        _audioChannelPair = (AudioChannelPair)(ushort)(_id & 0xFFFF);

        _name = rawCommand.ReadString(12, 64);
    }

    public void ApplyToState(AtemState state)
    {
        var output = state.GetFairlight().AudioRouting.Outputs.GetOrCreate(_id);

        output.Id =  _audioOutputId;
        output.ChannelPair = _audioChannelPair;
        output.InternalPortType = _internalPortType;
        output.ExternalPortType = _externalPortType;
        output.Name = _name;
    }
}
