using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Used to trigger the selected transition effect linear over the configured time
/// </summary>
[Command("DAut")]
[BufferSize(4)]
public partial class AutoTransitionCommand(MixEffect mixEffect) : SerializedCommand
{
    [SerializedField(0)] [NoProperty] private readonly byte _id = mixEffect.Id;
}
