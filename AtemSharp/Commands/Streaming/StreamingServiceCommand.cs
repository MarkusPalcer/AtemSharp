using AtemSharp.Lib;
using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Commands.Streaming;

[Command("CRSS", ProtocolVersion.V8_1_1)]
[BufferSize(1100)]
public partial class StreamingServiceCommand(AtemState state) : SerializedCommand
{
    [CustomSerialization(0)]
    private string _serviceName = state.Streaming.ServiceName;

    [CustomSerialization(1)]
    private string _url = state.Streaming.Url;

    [CustomSerialization(2)]
    private string _key = state.Streaming.Key;

    [SerializedField(1092, 3)] private uint _lowBitrate = state.Streaming.VideoBitrates.Low;
    [SerializedField(1096, 3)] private uint _highBitrate = state.Streaming.VideoBitrates.High;

    private void SerializeInternal(byte[] rawCommand)
    {
        rawCommand.WriteString(_serviceName, 1, 64);
        rawCommand.WriteString(_url, 65, 512);
        rawCommand.WriteString(_key, 577, 512);
    }
}
