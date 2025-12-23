using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Used to trigger a cut-transition
/// </summary>
[Command("DCut")]
[BufferSize(4)]
public partial class CutCommand(MixEffect mixEffect) : SerializedCommand
{
    [SerializedField(0)] [NoProperty] private readonly byte _id = mixEffect.Id;
}
