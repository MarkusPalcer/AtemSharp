using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Used to set the position of the current transition for a mix effect
/// </summary>
[Command("CTPs")]
[BufferSize(4)]
public partial class TransitionPositionCommand(MixEffect mixEffect) : SerializedCommand
{
    [SerializedField(0)] [NoProperty] private readonly byte _mixEffectId = mixEffect.Id;

    [SerializedField(2, 0)] [ScalingFactor(10000)] [SerializedType(typeof(ushort))]
    private double _handlePosition = mixEffect.TransitionPosition.HandlePosition;
}
