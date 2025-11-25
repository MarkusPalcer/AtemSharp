using AtemSharp.State.Video.MixEffect.UpstreamKeyer;

namespace AtemSharp.Commands.MixEffects.Key;

[Command("CKTp")]
[BufferSize(8)]
public partial class MixEffectKeyTypeSetCommand(UpstreamKeyer keyer) : SerializedCommand
{
    [SerializedField(1)] [NoProperty] private readonly byte _mixEffectId = keyer.MixEffectId;
    [SerializedField(2)] [NoProperty] private readonly byte _keyerId = keyer.Id;
    [SerializedField(3)] private MixEffectKeyType _keyType = keyer.KeyType;
    [SerializedField(4)] private bool _flyEnabled = keyer.FlyEnabled;
}
