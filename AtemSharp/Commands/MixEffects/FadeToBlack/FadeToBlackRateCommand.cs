using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.FadeToBlack;

[Command("FtbC")]
public class FadeToBlackRateCommand: SerializedCommand
{
    public byte MixEffectId { get; init; }
    public byte Rate { get; init; }

    public FadeToBlackRateCommand(AtemState state, int mixEffectId)
    {
        // TODO: Change to array access after array initializes from Topology command
        if (state.Info.Capabilities is null)
        {
            throw new InvalidOperationException("Fade to black rate cannot be applied before capabilities are known");
        }

        if (mixEffectId >= state.Info.Capabilities.MixEffects)
        {
            throw new IndexOutOfRangeException("Mix effect with index {MixEffectId} does not exist");
        }

        var mixEffect = state.Video.MixEffects.GetOrCreate(mixEffectId);
        mixEffect.Index = mixEffectId;

        if (mixEffect.FadeToBlack is null)
        {
            throw new InvalidOperationException("Can't set fade to black rate before fade to black properties are initialized");
        }

        MixEffectId = (byte)mixEffectId;
        Rate = mixEffect.FadeToBlack.Rate;
    }

    public override byte[] Serialize(ProtocolVersion version)
    {
        var buffer = new byte[4];
        buffer.WriteUInt8(1, 0);
        buffer.WriteUInt8(MixEffectId, 1);
        buffer.WriteUInt8(Rate, 2);
        return buffer;
    }
}
