using AtemSharp.Helpers;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Streaming;

[Command("CRSS")]
[BufferSize(1100)]
public partial class StreamingServiceCommand(AtemState state) : SerializedCommand
{
    [SerializedField(1, 0)]
    [CustomSerialization]
    private string _serviceName = state.Streaming.ServiceName;

    [SerializedField(65, 1)]
    [CustomSerialization]
    private string _url = state.Streaming.Url;

    [SerializedField(577, 2)]
    [CustomSerialization]
    private string _key = state.Streaming.Key;

    [SerializedField(1092, 3)] private uint _bitrate1 = state.Streaming.Bitrate1;
    [SerializedField(1096, 3)] private uint _bitrate2 = state.Streaming.Bitrate2;

    private void SerializeInternal(byte[] rawCommand)
    {
        rawCommand.WriteString(_serviceName, 1, 64);
        rawCommand.WriteString(_url, 65, 512);
        rawCommand.WriteString(_key, 577, 512);
    }
}
