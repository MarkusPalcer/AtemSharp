using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Commands.MixEffects.FadeToBlack;

/// <summary>
/// Used to set the duration of the FadeToBlack effect in frames
/// </summary>
[Command("FtbC")]
[BufferSize(4)]
public partial class FadeToBlackRateCommand(MixEffect mixEffect) : SerializedCommand
{
    [SerializedField(1)] [NoProperty] private readonly byte _mixEffectId = mixEffect.Id;

    [SerializedField(2)] private byte _rate = mixEffect.FadeToBlack.Rate;

    private void SerializeInternal(byte[] buffer)
    {
        // Flag is always 1
        buffer.WriteUInt8(1, 0);
    }
}
