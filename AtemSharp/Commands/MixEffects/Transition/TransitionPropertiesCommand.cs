using AtemSharp.State.Video.MixEffect;
using AtemSharp.State.Video.MixEffect.Transition;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Used to set which transition to use and which keyers to add in with the next transition for a MixEffect
/// </summary>
[Command("CTTp")]
[BufferSize(4)]
public partial class TransitionPropertiesCommand(MixEffect mixEffect) : SerializedCommand
{
    [SerializedField(1)] [NoProperty] private readonly byte _mixEffectId = mixEffect.Id;

    [SerializedField(2, 0)] private TransitionStyle _nextStyle = mixEffect.TransitionProperties.NextStyle;

    [SerializedField(3, 1)] private TransitionSelection _nextSelection = mixEffect.TransitionProperties.NextSelection;
}
