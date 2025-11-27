using AtemSharp.State.Video.MixEffect;
using AtemSharp.State.Video.MixEffect.Transition;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command to set DVE transition settings for a mix effect
/// </summary>
[Command("CTDv")]
[BufferSize(20)]
public partial class TransitionDigitalVideoEffectCommand(MixEffect mixEffect) : SerializedCommand
{
    [SerializedField(2)] [NoProperty] private readonly byte _mixEffectId = mixEffect.Id;

    /// <summary>
    /// Transition rate in frames
    /// </summary>
    [SerializedField(3, 0)] private byte _rate = mixEffect.TransitionSettings.DigitalVideoEffect.Rate;

    /// <summary>
    /// Logo/key transition rate in frames
    /// </summary>
    [SerializedField(4, 1)] private byte _logoRate = mixEffect.TransitionSettings.DigitalVideoEffect.LogoRate;

    /// <summary>
    /// DVE effect style
    /// </summary>
    [SerializedField(5, 2)] private DigitalVideoEffect _style = mixEffect.TransitionSettings.DigitalVideoEffect.Style;

    /// <summary>
    /// Fill source input number
    /// </summary>
    [SerializedField(6, 3)] private ushort _fillSource = mixEffect.TransitionSettings.DigitalVideoEffect.FillSource;

    /// <summary>
    /// Key source input number
    /// </summary>
    [SerializedField(8, 4)] private ushort _keySource = mixEffect.TransitionSettings.DigitalVideoEffect.KeySource;

    /// <summary>
    /// Whether the key is enabled
    /// </summary>
    [SerializedField(10, 5)] private bool _enableKey = mixEffect.TransitionSettings.DigitalVideoEffect.EnableKey;

    /// <summary>
    /// Whether the key is pre-multiplied
    /// </summary>
    [SerializedField(11, 6)] private bool _preMultiplied = mixEffect.TransitionSettings.DigitalVideoEffect.PreMultiplied;

    /// <summary>
    /// Key clip value (0.0 to 100.0)
    /// </summary>
    [SerializedField(12, 7)] [SerializedType(typeof(ushort))] [ScalingFactor(10)]
    private double _clip = mixEffect.TransitionSettings.DigitalVideoEffect.Clip;

    /// <summary>
    /// Key gain value (0.0 to 100.0)
    /// </summary>
    [SerializedField(14, 8)] [SerializedType(typeof(ushort))] [ScalingFactor(10)]
    private double _gain = mixEffect.TransitionSettings.DigitalVideoEffect.Gain;

    /// <summary>
    /// Whether the key is inverted
    /// </summary>
    [SerializedField(16, 9)] private bool _invertKey = mixEffect.TransitionSettings.DigitalVideoEffect.InvertKey;

    /// <summary>
    /// Whether the transition is reversed
    /// </summary>
    [SerializedField(17, 10)] private bool _reverse = mixEffect.TransitionSettings.DigitalVideoEffect.Reverse;

    /// <summary>
    /// Whether flip-flop is enabled
    /// </summary>
    [SerializedField(18, 11)] private bool _flipFlop = mixEffect.TransitionSettings.DigitalVideoEffect.FlipFlop;

    private void SerializeInternal(byte[] buffer)
    {
        buffer.WriteUInt16BigEndian((ushort)Flag, 0);
    }
}
