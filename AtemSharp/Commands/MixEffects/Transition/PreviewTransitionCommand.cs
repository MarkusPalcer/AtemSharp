using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Used to set transition preview state for a mix effect
/// </summary>
[Command("CTPr")]
[BufferSize(4)]
public partial class PreviewTransitionCommand(MixEffect mixEffect) : SerializedCommand
{
    [SerializedField(0)] [NoProperty] private readonly byte _mixEffectId = mixEffect.Id;

    [SerializedField(1)] private bool _preview = mixEffect.TransitionPreview;
}
