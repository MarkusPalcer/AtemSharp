using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Commands.Streaming;

[Command("STAB", ProtocolVersion.V8_1_1)]
internal partial class StreamingAudioBitratesUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private uint _lowBitrate;
    [DeserializedField(4)] private uint _highBitrate;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.Streaming.AudioBitrates.Low = _lowBitrate;
        state.Streaming.AudioBitrates.High = _highBitrate;
    }
}
