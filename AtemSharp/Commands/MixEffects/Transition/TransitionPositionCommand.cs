using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command to set the transition handle position for a mix effect
/// </summary>
[Command("CTPs")]
[BufferSize(4)]
public partial class TransitionPositionCommand(MixEffect mixEffect) : SerializedCommand
{
    [SerializedField(0)] [NoProperty] private readonly byte _mixEffectId = mixEffect.Id;

    /// <summary>
    /// The position of the transition handle (0.0 to 1.0, where 1.0 = 100%)
    /// </summary>
    [SerializedField(2, 0)] [ScalingFactor(10000)] [SerializedType(typeof(ushort))]
    private double _handlePosition = mixEffect.TransitionPosition.HandlePosition;
}
