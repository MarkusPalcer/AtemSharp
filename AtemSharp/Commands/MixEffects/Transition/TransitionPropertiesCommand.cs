using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command to set transition properties for a mix effect
/// </summary>
[Command("CTTp")]
[BufferSize(4)]
public partial class TransitionPropertiesCommand(MixEffect mixEffect) : SerializedCommand
{
    [SerializedField(1)] [NoProperty] internal readonly byte MixEffectId = mixEffect.Id;

    [SerializedField(2, 0)] private TransitionStyle _nextStyle = mixEffect.TransitionProperties.NextStyle;

    [SerializedField(3, 1)] private TransitionSelection _nextSelection = mixEffect.TransitionProperties.NextSelection;
}
