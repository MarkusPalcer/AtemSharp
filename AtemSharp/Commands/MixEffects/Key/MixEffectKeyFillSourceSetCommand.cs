using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Key;

[Command("CKeF")]
[BufferSize(4)]
public partial class MixEffectKeyFillSourceSetCommand(UpstreamKeyer keyer) : SerializedCommand
{
    [SerializedField(0)][NoProperty] private byte _mixEffectId = keyer.MixEffectId;
    [SerializedField(1)] [NoProperty] private byte _id = keyer.Id;
    [SerializedField(2)] private ushort _fillSourceId = keyer.FillSource;
}
