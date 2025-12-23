using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Commands.MixEffects.FadeToBlack;

/// <summary>
/// Used to toggle FadeToBlack
/// </summary>
[Command("FtbA")]
[BufferSize(4)]
public partial class FadeToBlackAutoCommand(MixEffect mixEffect) : SerializedCommand
{
    [SerializedField(0)] [NoProperty] private readonly byte _id = mixEffect.Id;
}
