using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command to set transition preview state for a mix effect
/// </summary>
[Command("CTPr")]
[BufferSize(4)]
public partial class PreviewTransitionCommand(MixEffect mixEffect) : SerializedCommand
{
    [SerializedField(0)] [NoProperty] internal readonly byte MixEffectId = mixEffect.Id;

    /// <summary>
    /// Whether transition preview is enabled
    /// </summary>
    [SerializedField(1)] private bool _preview = mixEffect.TransitionPreview;
}
