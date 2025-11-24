using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Key;

[Command("SFKF")]
[BufferSize(4)]
public partial class MixEffectKeyFlyKeyframeStoreCommand(UpstreamKeyerFlyKeyframe keyframe) : SerializedCommand
{
    [SerializedField(0)] [NoProperty] private readonly byte _mixEffectId = keyframe.MixEffectId;
    [SerializedField(1)] [NoProperty] private readonly byte _upstreamKeyerId = keyframe.UpstreamKeyerId;
    [SerializedField(2)] [NoProperty] private readonly byte _keyframeId = keyframe.Id;
}
