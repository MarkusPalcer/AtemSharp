using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command received from ATEM device containing wipe transition settings update
/// </summary>
[Command("TWpP")]
public partial class TransitionWipeUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Mix effect index (0-based)
    /// </summary>
    [DeserializedField(0)] private byte _mixEffectId;

    /// <summary>
    /// Rate of the wipe transition in frames
    /// </summary>
    [DeserializedField(1)] private byte _rate;

    /// <summary>
    /// Pattern for the wipe transition
    /// </summary>
    [DeserializedField(2)] private byte _pattern;

    /// <summary>
    /// Width of the wipe border as percentage (0-100%)
    /// </summary>
    [DeserializedField(4)] [SerializedType(typeof(ushort))] [ScalingFactor(100)]
    private double _borderWidth;

    /// <summary>
    /// Input source for the wipe border
    /// </summary>
    [DeserializedField(6)] private ushort _borderInput;

    /// <summary>
    /// Symmetry setting for the wipe transition as percentage (0-100%)
    /// </summary>
    [DeserializedField(8)] [SerializedType(typeof(ushort))] [ScalingFactor(100)]
    private double _symmetry;

    /// <summary>
    /// Softness of the wipe border as percentage (0-100%)
    /// </summary>
    [DeserializedField(10)] [SerializedType(typeof(ushort))] [ScalingFactor(100)]
    private double _borderSoftness;

    /// <summary>
    /// X position for the wipe transition (0.0-1.0)
    /// </summary>
    [DeserializedField(12)] [SerializedType(typeof(ushort))] [ScalingFactor(10000)]
    private double _xPosition;

    /// <summary>
    /// Y position for the wipe transition (0.0-1.0)
    /// </summary>
    [DeserializedField(14)] [SerializedType(typeof(ushort))] [ScalingFactor(10000)]
    private double _yPosition;

    /// <summary>
    /// Whether the wipe direction is reversed
    /// </summary>
    [DeserializedField(16)] private bool _reverseDirection;

    /// <summary>
    /// Whether flip flop mode is enabled
    /// </summary>
    [DeserializedField(17)] private bool _flipFlop;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var mixEffect = state.Video.MixEffects[MixEffectId];
        mixEffect.TransitionSettings.Wipe.Rate = Rate;
        mixEffect.TransitionSettings.Wipe.Pattern = Pattern;
        mixEffect.TransitionSettings.Wipe.BorderWidth = BorderWidth;
        mixEffect.TransitionSettings.Wipe.BorderInput = BorderInput;
        mixEffect.TransitionSettings.Wipe.Symmetry = Symmetry;
        mixEffect.TransitionSettings.Wipe.BorderSoftness = BorderSoftness;
        mixEffect.TransitionSettings.Wipe.XPosition = XPosition;
        mixEffect.TransitionSettings.Wipe.YPosition = YPosition;
        mixEffect.TransitionSettings.Wipe.ReverseDirection = ReverseDirection;
        mixEffect.TransitionSettings.Wipe.FlipFlop = FlipFlop;
    }
}
