using AtemSharp.Helpers;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.FadeToBlack;

[Command("FtbC")]
[BufferSize(4)]
public partial class FadeToBlackRateCommand: SerializedCommand
{
    [SerializedField(1)]
    [NoProperty]
    private readonly byte _mixEffectId;

    [SerializedField(2)]
    private byte _rate;

    private void SerializeInternal(byte[] buffer)
    {
        // Flag is always 1
        buffer.WriteUInt8(1, 0);
    }

    public FadeToBlackRateCommand(MixEffect mixEffect)
    {
        if (mixEffect.FadeToBlack is null)
        {
            throw new InvalidOperationException("Can't set fade to black rate before fade to black properties are initialized");
        }

        _mixEffectId = mixEffect.Index;
        Rate = mixEffect.FadeToBlack.Rate;
    }
}
