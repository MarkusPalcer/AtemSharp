using AtemSharp.Helpers;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command received from ATEM device containing stinger transition settings update
/// </summary>
[Command("TStP")]
public partial class TransitionStingerUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private byte _mixEffectId;

    /// <summary>
    /// Source for the stinger transition
    /// </summary>
    [DeserializedField(1)]
    private byte _source;

    /// <summary>
    /// Whether the key is pre-multiplied
    /// </summary>
    [DeserializedField(2)]
    private bool _preMultipliedKey;

    /// <summary>
    /// Clip value for the stinger transition
    /// </summary>
    [DeserializedField(4)]
    [SerializedType(typeof(ushort))]
    [ScalingFactor(10)]
    private double _clip;

    /// <summary>
    /// Gain value for the stinger transition (0-100%)
    /// </summary>
    [DeserializedField(6)]
    [SerializedType(typeof(ushort))]
    [ScalingFactor(10)]
    private double _gain;

    /// <summary>
    /// Whether the stinger transition is inverted
    /// </summary>
    [DeserializedField(8)]
    private bool _invert;

    /// <summary>
    /// Preroll value for the stinger transition
    /// </summary>
    [DeserializedField(10)]
    private ushort _preroll;

    /// <summary>
    /// Clip duration for the stinger transition
    /// </summary>
    [DeserializedField(12)]
    private ushort _clipDuration;

    /// <summary>
    /// Trigger point for the stinger transition
    /// </summary>
    [DeserializedField(14)]
    private ushort _triggerPoint;

    /// <summary>
    /// Mix rate for the stinger transition
    /// </summary>
    [DeserializedField(16)]
    private ushort _mixRate;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Validate mix effect index
        if (!state.Video.MixEffects.TryGetValue(MixEffectId, out var mixEffect))
        {
            throw new InvalidIdError("MixEffect", MixEffectId.ToString());
        }

        // Update the stinger settings
        mixEffect.TransitionSettings.Stinger.Source = Source;
        mixEffect.TransitionSettings.Stinger.PreMultipliedKey = PreMultipliedKey;
        mixEffect.TransitionSettings.Stinger.Clip = Clip;
        mixEffect.TransitionSettings.Stinger.Gain = Gain;
        mixEffect.TransitionSettings.Stinger.Invert = Invert;
        mixEffect.TransitionSettings.Stinger.Preroll = Preroll;
        mixEffect.TransitionSettings.Stinger.ClipDuration = ClipDuration;
        mixEffect.TransitionSettings.Stinger.TriggerPoint = TriggerPoint;
        mixEffect.TransitionSettings.Stinger.MixRate = MixRate;
    }
}
