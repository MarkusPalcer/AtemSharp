using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.FadeToBlack;

[Command("FtbP")]
public class FadeToBlackRateUpdateCommand : IDeserializedCommand
{
    public byte MixEffectId { get; init; }
    public byte Rate { get; init; }

    public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> data, ProtocolVersion version)
    {
        return new FadeToBlackRateUpdateCommand
        {
            MixEffectId = data.ReadUInt8(0),
            Rate = data.ReadUInt8(1)
        };
    }

    public void ApplyToState(AtemState state)
    {
        // TODO: Change to array access after array initializes from Topology command
        if (state.Info.Capabilities is null)
        {
            throw new InvalidOperationException("Fade to black rate cannot be applied before capabilities are known");
        }

        if (MixEffectId >= state.Info.Capabilities.MixEffects)
        {
            throw new IndexOutOfRangeException("Mix effect with index {MixEffectId} does not exist");
        }

        var mixEffect = state.Video.MixEffects.GetOrCreate(MixEffectId);
        mixEffect.Index = MixEffectId;
        mixEffect.FadeToBlack ??= new FadeToBlackProperties();
        mixEffect.FadeToBlack.Rate = Rate;
        mixEffect.FadeToBlack.IsFullyBlack = false;
        mixEffect.FadeToBlack.InTransition = false;
        mixEffect.FadeToBlack.RemainingFrames = 0;
    }
}
