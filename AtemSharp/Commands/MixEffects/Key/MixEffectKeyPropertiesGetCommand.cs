using AtemSharp.Enums;
using AtemSharp.Helpers;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Key;

/// <summary>
/// Command received from ATEM device containing upstream keyer base properties
/// </summary>
[Command("KeBP")]
public partial class MixEffectKeyPropertiesGetCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private byte _mixEffectIndex;

    [DeserializedField(1)]
    private byte _keyerIndex;

    [DeserializedField(2)]
    private MixEffectKeyType _keyType;

    [DeserializedField(4)]
    private bool _canFlyKey;

    [DeserializedField(5)]
    private bool _flyEnabled;

    [DeserializedField(6)]
    private ushort _fillSource;

    [DeserializedField(8)]
    private ushort _cutSource;

    [DeserializedField(10)]
    private bool _maskEnabled;

    [DeserializedField(12)]
    [ScalingFactor(1000)]
    [SerializedType(typeof(short))]
    private double _maskTop;

    [DeserializedField(14)]
    [ScalingFactor(1000)]
    [SerializedType(typeof(short))]
    private double _maskBottom;

    [DeserializedField(16)]
    [ScalingFactor(1000)]
    [SerializedType(typeof(short))]
    private double _maskLeft;

    [DeserializedField(18)]
    [ScalingFactor(1000)]
    [SerializedType(typeof(short))]
    private double _maskRight;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Validate mix effect index - need to get capabilities info
        if (state.Info.Capabilities == null || MixEffectIndex >= state.Info.Capabilities.MixEffects)
        {
            throw new InvalidIdError("MixEffect", MixEffectIndex);
        }

        // TODO: Add validation for keyer index when capabilities include upstream keyer count
        // For now, we'll proceed with state updates

        // Get or create the mix effect
        var mixEffect = state.Video.MixEffects.GetOrCreate(MixEffectIndex);

        // Get or create the upstream keyer
        var keyer = mixEffect.UpstreamKeyers.GetOrCreate(KeyerIndex);
        keyer.Id = KeyerIndex;

        // Update keyer properties
        keyer.KeyType = KeyType;
        keyer.CanFlyKey = CanFlyKey;
        keyer.FlyEnabled = FlyEnabled;
        keyer.FillSource = FillSource;
        keyer.CutSource = CutSource;

        // Update mask settings
        keyer.MaskSettings.MaskEnabled = MaskEnabled;
        keyer.MaskSettings.MaskTop = MaskTop;
        keyer.MaskSettings.MaskBottom = MaskBottom;
        keyer.MaskSettings.MaskLeft = MaskLeft;
        keyer.MaskSettings.MaskRight = MaskRight;
    }
}
