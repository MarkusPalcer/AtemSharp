using AtemSharp.Enums;
using AtemSharp.Helpers;
using AtemSharp.State;

namespace AtemSharp.Commands.Streaming;

[Command("STAB", ProtocolVersion.V8_1_1)]
public partial class StreamingAudioBitratesUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private uint lowBitrate;

    [DeserializedField(4)]
    private uint highBitrate;

    public void ApplyToState(AtemState state)
    {
        state.Streaming.AudioBitrates.Low = lowBitrate;
        state.Streaming.AudioBitrates.High = highBitrate;
    }
}
