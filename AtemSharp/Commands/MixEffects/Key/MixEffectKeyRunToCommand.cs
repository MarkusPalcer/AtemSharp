using AtemSharp.State.Video.MixEffect.UpstreamKeyer;

namespace AtemSharp.Commands.MixEffects.Key;

[Command("RFlK")]
[BufferSize(8)]
public partial class MixEffectKeyRunToCommand(UpstreamKeyerFlyKeyframe keyframe) : SerializedCommand
{
    [SerializedField(1)] [NoProperty] private readonly byte _mixEffectId = keyframe.MixEffectId;

    [SerializedField(2)] [NoProperty] private readonly byte _keyerId = keyframe.UpstreamKeyerId;

    [SerializedField(4)] [NoProperty] private readonly byte _keyframeId = keyframe.Id;

    [SerializedField(5)] private FlyKeyDirection _direction = FlyKeyDirection.CentreOfKey;

    // TODO #83: Capture test data for this case
    /// <summary>
    /// Creates a new command that sends the keyer to "RunToInfinite"
    /// </summary>
    public static MixEffectKeyRunToCommand RunToInfinite(UpstreamKeyer keyer)
    {
        return new MixEffectKeyRunToCommand(new UpstreamKeyerFlyKeyframe
        {
            MixEffectId = keyer.MixEffectId,
            Id = 4
        });
    }

    private void SerializeInternal(byte[] buffer)
    {
        buffer.WriteUInt8((byte)(_keyframeId == 4 ? 2 : 0), 0);
    }
}
