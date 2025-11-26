using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command to set stinger transition settings for a mix effect
/// </summary>
[Command("CTSt")]
[BufferSize(20)]
public partial class TransitionStingerCommand(MixEffect mixEffect) : SerializedCommand
{
    [SerializedField(2)] [NoProperty] internal readonly byte MixEffectId = mixEffect.Id;

    /// <summary>
    /// Source for the stinger transition
    /// </summary>
    [SerializedField(3, 0)] private byte _source = mixEffect.TransitionSettings.Stinger.Source;

    /// <summary>
    /// Whether the key is pre-multiplied
    /// </summary>
    [SerializedField(4, 1)] private bool _preMultipliedKey = mixEffect.TransitionSettings.Stinger.PreMultipliedKey;

    /// <summary>
    /// Clip value for the stinger transition
    /// </summary>
    [SerializedField(6, 2)] [SerializedType(typeof(ushort))] [ScalingFactor(10)]
    private double _clip = mixEffect.TransitionSettings.Stinger.Clip;

    /// <summary>
    /// Gain value for the stinger transition (0-100%)
    /// </summary>
    [SerializedField(8, 3)] [SerializedType(typeof(ushort))] [ScalingFactor(10)]
    private double _gain = mixEffect.TransitionSettings.Stinger.Gain;

    /// <summary>
    /// Whether the stinger transition is inverted
    /// </summary>
    [SerializedField(10, 4)] private bool _invert = mixEffect.TransitionSettings.Stinger.Invert;

    /// <summary>
    /// Preroll value for the stinger transition
    /// </summary>
    [SerializedField(12, 5)] private ushort _preroll = mixEffect.TransitionSettings.Stinger.Preroll;

    /// <summary>
    /// Clip duration for the stinger transition
    /// </summary>
    [SerializedField(14, 6)] private ushort _clipDuration = mixEffect.TransitionSettings.Stinger.ClipDuration;

    /// <summary>
    /// Trigger point for the stinger transition
    /// </summary>
    [SerializedField(16, 7)] private ushort _triggerPoint = mixEffect.TransitionSettings.Stinger.TriggerPoint;

    /// <summary>
    /// Mix rate for the stinger transition
    /// </summary>
    [SerializedField(18, 8)] private ushort _mixRate = mixEffect.TransitionSettings.Stinger.MixRate;

    private void SerializeInternal(byte[] buffer)
    {
        buffer.WriteUInt16BigEndian((ushort)Flag, 0);
    }
}
