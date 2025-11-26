using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Commands.Streaming;

[Command("SRSS", ProtocolVersion.V8_1_1)]
public partial class StreamingStatsUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)]  private uint _encodingBitrate;
    [DeserializedField(4)] private ushort _cacheUsed;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.Streaming.EncodingBitrate = _encodingBitrate;
        state.Streaming.CacheUsed = _cacheUsed;
    }
}
