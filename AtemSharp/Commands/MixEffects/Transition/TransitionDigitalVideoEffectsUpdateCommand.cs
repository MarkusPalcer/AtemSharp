using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command received from ATEM device containing DVE transition settings update
/// </summary>
[Command("TDvP")]
// ReSharper disable once InconsistentNaming Domain Specific Acronym
public partial class TransitionDigitalVideoEffectsUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _mixEffectId;

    /// <summary>
    /// Transition rate in frames
    /// </summary>
    [DeserializedField(1)] private byte _rate;

    /// <summary>
    /// Logo/key transition rate in frames
    /// </summary>
    [DeserializedField(2)] private byte _logoRate;

    /// <summary>
    /// DVE effect style
    /// </summary>
    [DeserializedField(3)] private DigitalVideoEffect _style;

    /// <summary>
    /// Fill source input number
    /// </summary>
    [DeserializedField(4)] private ushort _fillSource;

    /// <summary>
    /// Key source input number
    /// </summary>
    [DeserializedField(6)] private ushort _keySource;

    /// <summary>
    /// Whether the key is enabled
    /// </summary>
    [DeserializedField(8)] private bool _enableKey;

    /// <summary>
    /// Whether the key is pre-multiplied
    /// </summary>
    [DeserializedField(9)] private bool _preMultiplied;

    /// <summary>
    /// Key clip value (0.0 to 100.0)
    /// </summary>
    [DeserializedField(10)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _clip;

    /// <summary>
    /// Key gain value (0.0 to 100.0)
    /// </summary>
    [DeserializedField(12)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _gain;

    /// <summary>
    /// Whether the key is inverted
    /// </summary>
    [DeserializedField(14)] private bool _invertKey;

    /// <summary>
    /// Whether the transition is reversed
    /// </summary>
    [DeserializedField(15)] private bool _reverse;

    /// <summary>
    /// Whether flip-flop is enabled
    /// </summary>
    [DeserializedField(16)] private bool _flipFlop;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var mixEffect = state.Video.MixEffects[_mixEffectId];
        mixEffect.TransitionSettings.DigitalVideoEffect.Rate = Rate;
        mixEffect.TransitionSettings.DigitalVideoEffect.LogoRate = LogoRate;
        mixEffect.TransitionSettings.DigitalVideoEffect.Style = Style;
        mixEffect.TransitionSettings.DigitalVideoEffect.FillSource = FillSource;
        mixEffect.TransitionSettings.DigitalVideoEffect.KeySource = KeySource;
        mixEffect.TransitionSettings.DigitalVideoEffect.EnableKey = EnableKey;
        mixEffect.TransitionSettings.DigitalVideoEffect.PreMultiplied = PreMultiplied;
        mixEffect.TransitionSettings.DigitalVideoEffect.Clip = Clip;
        mixEffect.TransitionSettings.DigitalVideoEffect.Gain = Gain;
        mixEffect.TransitionSettings.DigitalVideoEffect.InvertKey = InvertKey;
        mixEffect.TransitionSettings.DigitalVideoEffect.Reverse = Reverse;
        mixEffect.TransitionSettings.DigitalVideoEffect.FlipFlop = FlipFlop;
    }
}
