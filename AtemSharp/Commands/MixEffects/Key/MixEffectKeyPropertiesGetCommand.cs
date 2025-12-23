using AtemSharp.State;
using AtemSharp.State.Video.MixEffect.UpstreamKeyer;

namespace AtemSharp.Commands.MixEffects.Key;

[Command("KeBP")]
internal partial class MixEffectKeyPropertiesGetCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _mixEffectIndex;

    [DeserializedField(1)] private byte _keyerIndex;

    [DeserializedField(2)] private MixEffectKeyType _keyType;

    [DeserializedField(4)] private bool _canFlyKey;

    [DeserializedField(5)] private bool _flyEnabled;

    [DeserializedField(6)] private ushort _fillSource;

    [DeserializedField(8)] private ushort _cutSource;

    [DeserializedField(10)] private bool _maskEnabled;

    [DeserializedField(12)] [ScalingFactor(1000)] [SerializedType(typeof(short))]
    private double _maskTop;

    [DeserializedField(14)] [ScalingFactor(1000)] [SerializedType(typeof(short))]
    private double _maskBottom;

    [DeserializedField(16)] [ScalingFactor(1000)] [SerializedType(typeof(short))]
    private double _maskLeft;

    [DeserializedField(18)] [ScalingFactor(1000)] [SerializedType(typeof(short))]
    private double _maskRight;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var mixEffect = state.Video.MixEffects[MixEffectIndex];
        var keyer = mixEffect.UpstreamKeyers.GetOrCreate(KeyerIndex);

        // Update keyer properties
        keyer.KeyType = KeyType;
        keyer.CanFlyKey = CanFlyKey;
        keyer.FlyEnabled = FlyEnabled;
        keyer.FillSource = FillSource;
        keyer.CutSource = CutSource;

        // Update mask settings
        keyer.Mask.Enabled = MaskEnabled;
        keyer.Mask.Top = MaskTop;
        keyer.Mask.Bottom = MaskBottom;
        keyer.Mask.Left = MaskLeft;
        keyer.Mask.Right = MaskRight;
    }
}
