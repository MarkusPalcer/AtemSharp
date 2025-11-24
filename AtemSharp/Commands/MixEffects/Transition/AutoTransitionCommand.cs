using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

[Command("DAut")]
[BufferSize(4)]
public partial class AutoTransitionCommand(MixEffect mixEffect) : SerializedCommand
{
    [SerializedField(0)] [NoProperty] private readonly byte _id = mixEffect.Index;
}
