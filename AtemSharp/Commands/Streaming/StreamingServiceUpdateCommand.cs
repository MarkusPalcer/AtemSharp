using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Commands.Streaming;

[Command("SRSU", ProtocolVersion.V8_1_1)]
public partial class StreamingServiceUpdateCommand : IDeserializedCommand
{
    [CustomDeserialization] private string _serviceName = string.Empty;

    [CustomDeserialization] private string _url = string.Empty;

    [CustomDeserialization] private string _key = string.Empty;

    [DeserializedField(1088)] private uint _bitrate1;
    [DeserializedField(1092)] private uint _bitrate2;

    private void DeserializeInternal(ReadOnlySpan<byte> rawCommand)
    {
        _serviceName = rawCommand.ReadString(0, 64);
        _url = rawCommand.ReadString(64, 512);
        _key = rawCommand.ReadString(576, 512);
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.Streaming.ServiceName = _serviceName;
        state.Streaming.Url = _url;
        state.Streaming.Key = _key;
        state.Streaming.VideoBitrates.Low = _bitrate1;
        state.Streaming.VideoBitrates.High = _bitrate2;
    }
}
