using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Used to set dip transition settings for a mix effect
/// </summary>
[Command("CTDp")]
[BufferSize(8)]
public partial class TransitionDipCommand(MixEffect mixEffect) : SerializedCommand
{
    [SerializedField(1)] [NoProperty] private readonly byte _mixEffectId = mixEffect.Id;

    [SerializedField(2, 0)] private byte _rate = mixEffect.TransitionSettings.Dip.Rate;

    [SerializedField(4, 1)] private ushort _input = mixEffect.TransitionSettings.Dip.Input;
}
