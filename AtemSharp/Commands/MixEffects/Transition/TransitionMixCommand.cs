using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Used to set the duration of the mix transition for a MixEffect
/// </summary>
[Command("CTMx")]
[BufferSize(4)]
public partial class TransitionMixCommand(MixEffect mixEffect) : SerializedCommand
{
    [SerializedField(0)] [NoProperty]
    private readonly byte _mixEffectId = mixEffect.Id;

    [SerializedField(1)]
    private byte _rate = mixEffect.TransitionSettings.Mix.Rate;
}
