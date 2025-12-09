using AtemSharp.State.Video.MixEffect.UpstreamKeyer;

namespace AtemSharp.Commands.MixEffects.Key;

[Command("RFlK")]
[BufferSize(8)]
public partial class MixEffectKeyRunToCommand : SerializedCommand
{
    [SerializedField(1)] [NoProperty] private readonly byte _mixEffectId;

    [SerializedField(2)] [NoProperty] private readonly byte _keyerId;

    [SerializedField(4)] [NoProperty] private readonly byte _keyframeId;

    [SerializedField(5)] [NoProperty] private readonly FlyKeyDirection _direction;

    private MixEffectKeyRunToCommand(byte mixEffectId, byte keyerId, byte keyframeId, FlyKeyDirection direction)
    {
        _mixEffectId = mixEffectId;
        _keyerId = keyerId;
        _keyframeId = keyframeId;
        _direction = direction;
    }

    /// <summary>
    /// Creates a new command that sends the keyer into the given direction until it is invisible
    /// </summary>
    public static MixEffectKeyRunToCommand RunToInfinite(UpstreamKeyer keyer, FlyKeyDirection direction)
    {
        return new MixEffectKeyRunToCommand(keyer.MixEffectId, keyer.Id, 4, direction);
    }

    /// <summary>
    /// Creates a new command that animates the keyer to full screen
    /// </summary>
    public static MixEffectKeyRunToCommand RunToFull(UpstreamKeyer keyer) => RunToFull(keyer, FlyKeyDirection.CentreOfKey);

    // This exists because the tests insist on a direction to be serialized even if no direction makes sense in this case
    internal static MixEffectKeyRunToCommand RunToFull(UpstreamKeyer keyer, FlyKeyDirection direction)
    {
        return new MixEffectKeyRunToCommand(keyer.MixEffectId, keyer.Id, 3, direction);
    }

    /// <summary>
    /// Creates a new command that animates the keyer to the given keyframe
    /// </summary>
    public static MixEffectKeyRunToCommand RunToKeyframe(UpstreamKeyerFlyKeyframe keyframe) =>
        RunToKeyframe(keyframe, FlyKeyDirection.CentreOfKey);

    // This exists because the tests insist on a direction to be serialized even if no direction makes sense in this case
    internal static MixEffectKeyRunToCommand RunToKeyframe(UpstreamKeyerFlyKeyframe keyframe, FlyKeyDirection direction)
    {
        return new MixEffectKeyRunToCommand(keyframe.MixEffectId, keyframe.UpstreamKeyerId, keyframe.Id, direction);
    }

    private void SerializeInternal(byte[] buffer)
    {
        buffer.WriteUInt8((byte)(_keyframeId == 4 ? 2 : 0), 0);
    }
}
