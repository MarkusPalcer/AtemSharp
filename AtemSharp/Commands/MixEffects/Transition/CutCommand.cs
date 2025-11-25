using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

[Command("DCut")]
[BufferSize(4)]
public partial class CutCommand(MixEffect mixEffect) : SerializedCommand
{
    [SerializedField(0)] [NoProperty] private readonly byte _id = mixEffect.Id;
}
