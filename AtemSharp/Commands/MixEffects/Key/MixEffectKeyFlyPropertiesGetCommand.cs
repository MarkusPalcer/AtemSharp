using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Key;

/// <summary>
/// Command received from ATEM device containing upstream keyer fly properties
/// </summary>
[Command("KeFS")]
public partial class MixEffectKeyFlyPropertiesGetCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _mixEffectIndex;

    [DeserializedField(1)] private byte _keyerIndex;

    /// <summary>
    /// Whether key frame A is set
    /// </summary>
    [DeserializedField(2)] private bool _isASet;

    /// <summary>
    /// Whether key frame B is set
    /// </summary>
    [DeserializedField(3)] private bool _isBSet;

    /// <summary>
    /// Running to key frame flags
    /// </summary>
    [DeserializedField(4)] private IsAtKeyFrame _isAtKeyFrame;

    /// <summary>
    /// Running to infinite index
    /// </summary>
    [DeserializedField(5)] private byte _runToInfiniteIndex;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Get or create the mix effect
        var mixEffect = state.Video.MixEffects[MixEffectIndex];

        // Get or create the upstream keyer
        var keyer = mixEffect.UpstreamKeyers.GetOrCreate(KeyerIndex);
        keyer.Id = KeyerIndex;

        keyer.FlyProperties.IsASet = IsASet;
        keyer.FlyProperties.IsBSet = IsBSet;
        keyer.FlyProperties.IsAtKeyFrame = IsAtKeyFrame;
        keyer.FlyProperties.RunToInfiniteIndex = RunToInfiniteIndex;
    }
}
