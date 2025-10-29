using AtemSharp.Enums;
using AtemSharp.Helpers;
using AtemSharp.State;

namespace AtemSharp.Commands.Streaming;

[Command("STAB", ProtocolVersion.V8_1_1)]
[BufferSize(8)]
public partial class StreamingAudioBitratesCommand(AtemState state) : SerializedCommand
{
    [SerializedField(0)]
    private uint _lowBitrate = state.Streaming.AudioBitrateLow;

    [SerializedField(4)]
    private uint _highBitrate = state.Streaming.AudioBitrateHigh;
}
