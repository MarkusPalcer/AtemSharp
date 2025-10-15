using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.FadeToBlack;

[Command("FtbS")]
public class FadeToBlackStateCommand : IDeserializedCommand
{
    public byte MixEffectId { get; init; }
    public bool IsFullyBlack { get; init; }
    public bool InTransition { get; init; }
    public byte RemainingFrames { get; init; }

    public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> data, ProtocolVersion version)
    {
        return new FadeToBlackStateCommand()
        {
            MixEffectId = data.ReadUInt8(0),
            IsFullyBlack = data.ReadBoolean(1),
            InTransition = data.ReadBoolean(2),
            RemainingFrames = data.ReadUInt8(3)
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
        mixEffect.FadeToBlack.IsFullyBlack = IsFullyBlack;
        mixEffect.FadeToBlack.InTransition = InTransition;
        mixEffect.FadeToBlack.RemainingFrames = RemainingFrames;
    }
}
