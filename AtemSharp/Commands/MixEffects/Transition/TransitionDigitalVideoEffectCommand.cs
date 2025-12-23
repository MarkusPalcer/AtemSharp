using AtemSharp.State.Video.MixEffect;
using AtemSharp.State.Video.MixEffect.Transition;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Used to set DVE transition settings for a mix effect
/// </summary>
[Command("CTDv")]
[BufferSize(20)]
public partial class TransitionDigitalVideoEffectCommand(MixEffect mixEffect) : SerializedCommand
{
    [SerializedField(2)] [NoProperty] private readonly byte _mixEffectId = mixEffect.Id;

    [SerializedField(3, 0)] private byte _rate = mixEffect.TransitionSettings.DigitalVideoEffect.Rate;
    [SerializedField(4, 1)] private byte _logoRate = mixEffect.TransitionSettings.DigitalVideoEffect.LogoRate;

    [SerializedField(5, 2)] private DigitalVideoEffect _style = mixEffect.TransitionSettings.DigitalVideoEffect.Style;

    [SerializedField(6, 3)] private ushort _fillSource = mixEffect.TransitionSettings.DigitalVideoEffect.FillSource;

    [SerializedField(8, 4)] private ushort _keySource = mixEffect.TransitionSettings.DigitalVideoEffect.KeySource;

    [SerializedField(10, 5)] private bool _enableKey = mixEffect.TransitionSettings.DigitalVideoEffect.EnableKey;

    [SerializedField(11, 6)] private bool _preMultiplied = mixEffect.TransitionSettings.DigitalVideoEffect.PreMultipliedKey.Enabled;

    [SerializedField(12, 7)] [SerializedType(typeof(ushort))] [ScalingFactor(10)]
    private double _clip = mixEffect.TransitionSettings.DigitalVideoEffect.PreMultipliedKey.Clip;

    [SerializedField(14, 8)] [SerializedType(typeof(ushort))] [ScalingFactor(10)]
    private double _gain = mixEffect.TransitionSettings.DigitalVideoEffect.PreMultipliedKey.Gain;

    [SerializedField(16, 9)] private bool _invertKey = mixEffect.TransitionSettings.DigitalVideoEffect.PreMultipliedKey.Inverted;

    [SerializedField(17, 10)] private bool _reverse = mixEffect.TransitionSettings.DigitalVideoEffect.Reverse;

    [SerializedField(18, 11)] private bool _flipFlop = mixEffect.TransitionSettings.DigitalVideoEffect.FlipFlop;

    private void SerializeInternal(byte[] buffer)
    {
        buffer.WriteUInt16BigEndian((ushort)Flag, 0);
    }
}
