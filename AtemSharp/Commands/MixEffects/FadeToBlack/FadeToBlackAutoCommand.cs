using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.FadeToBlack;

[Command("FtbA")]
[BufferSize(4)]
public partial class FadeToBlackAutoCommand(MixEffect mixEffect) : SerializedCommand
{
    [SerializedField(0)] [NoProperty] private byte _id = mixEffect.Index;
}
