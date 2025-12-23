using AtemSharp.State.Video.MixEffect.UpstreamKeyer;

namespace AtemSharp.Commands.MixEffects.Key;

/// <summary>
/// Used to set the source for the transparency channel of the upstream keyer
/// </summary>
[Command("CKeC")]
[BufferSize(4)]
public partial class MixEffectKeyCutSourceSetCommand(UpstreamKeyer keyer) : SerializedCommand
{
    [SerializedField(0)] [NoProperty] private readonly byte _mixEffectId = keyer.MixEffectId;
    [SerializedField(1)] [NoProperty] private readonly byte _id = keyer.Id;
    [SerializedField(2)] private ushort _cutSourceId = keyer.CutSource;
}
