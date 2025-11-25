using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Commands.Streaming;

[Command("STAB", ProtocolVersion.V8_1_1)]
[BufferSize(8)]
public partial class StreamingAudioBitratesCommand(AtemState state) : SerializedCommand
{
    [SerializedField(0)]
    private uint _lowBitrate = state.Streaming.AudioBitrates.Low;

    [SerializedField(4)]
    private uint _highBitrate = state.Streaming.AudioBitrates.High;
}
