using AtemSharp.State;
using AtemSharp.State.Video.MixEffect.UpstreamKeyer;

namespace AtemSharp.Commands.MixEffects.Key;

[Command("KeFS")]
internal partial class MixEffectKeyFlyPropertiesUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _mixEffectIndex;

    [DeserializedField(1)] private byte _keyerIndex;

    [DeserializedField(2)] private bool _isASet;

    [DeserializedField(3)] private bool _isBSet;

    [DeserializedField(4)] private IsAtKeyFrame _isAtKeyFrame;

    [DeserializedField(5)] private byte _runToInfiniteIndex;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Get or create the mix effect
        var mixEffect = state.Video.MixEffects[MixEffectIndex];

        // Get or create the upstream keyer
        var keyer = mixEffect.UpstreamKeyers.GetOrCreate(KeyerIndex);

        keyer.FlyProperties.IsASet = IsASet;
        keyer.FlyProperties.IsBSet = IsBSet;
        keyer.FlyProperties.IsAtKeyFrame = IsAtKeyFrame;
        keyer.FlyProperties.RunToInfiniteIndex = RunToInfiniteIndex;
    }
}
