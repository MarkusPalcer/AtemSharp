using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command to set dip transition settings for a mix effect
/// </summary>
[Command("CTDp")]
[BufferSize(8)]
public partial class TransitionDipCommand(MixEffect mixEffect) : SerializedCommand
{
    [SerializedField(1)] [NoProperty] internal readonly byte MixEffectId = mixEffect.Id;

    /// <summary>
    /// Rate of the dip transition in frames
    /// </summary>
    [SerializedField(2, 0)] private byte _rate = mixEffect.TransitionSettings.Dip.Rate;

    /// <summary>
    /// Input source for the dip transition
    /// </summary>
    [SerializedField(4, 1)] private ushort _input = mixEffect.TransitionSettings.Dip.Input;
}
