using AtemSharp.Lib;
using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command to set wipe transition settings for a mix effect
/// </summary>
[Command("CTWp")]
[BufferSize(20)]
public partial class TransitionWipeCommand(MixEffect mixEffect) : SerializedCommand
{
    [SerializedField(2)] [NoProperty] internal readonly byte MixEffectId = mixEffect.Id;

    /// <summary>
    /// Rate of the wipe transition in frames
    /// </summary>
    [SerializedField(3, 0)] private byte _rate = mixEffect.TransitionSettings.Wipe.Rate;

    /// <summary>
    /// Pattern for the wipe transition
    /// </summary>
    [SerializedField(4, 1)] private byte _pattern = mixEffect.TransitionSettings.Wipe.Pattern;

    /// <summary>
    /// Width of the wipe border as percentage (0-100%)
    /// </summary>
    [SerializedField(6, 2)] [SerializedType(typeof(ushort))] [ScalingFactor(100)]
    private double _borderWidth = mixEffect.TransitionSettings.Wipe.BorderWidth;

    /// <summary>
    /// Input source for the wipe border
    /// </summary>
    [SerializedField(8, 3)] private ushort _borderInput = mixEffect.TransitionSettings.Wipe.BorderInput;

    /// <summary>
    /// Symmetry setting for the wipe transition as percentage (0-100%)
    /// </summary>
    [SerializedField(10, 4)] [SerializedType(typeof(ushort))] [ScalingFactor(100)]
    private double _symmetry = mixEffect.TransitionSettings.Wipe.Symmetry;

    /// <summary>
    /// Softness of the wipe border as percentage (0-100%)
    /// </summary>
    [SerializedField(12, 5)] [SerializedType(typeof(ushort))] [ScalingFactor(100)]
    private double _borderSoftness = mixEffect.TransitionSettings.Wipe.BorderSoftness;

    /// <summary>
    /// X position for the wipe transition (0.0-1.0)
    /// </summary>
    [SerializedField(14, 6)] [SerializedType(typeof(ushort))] [ScalingFactor(10000)]
    private double _xPosition = mixEffect.TransitionSettings.Wipe.XPosition;

    /// <summary>
    /// Y position for the wipe transition (0.0-1.0)
    /// </summary>
    [SerializedField(16, 7)] [SerializedType(typeof(ushort))] [ScalingFactor(10000)]
    private double _yPosition = mixEffect.TransitionSettings.Wipe.YPosition;

    /// <summary>
    /// Whether the wipe direction is reversed
    /// </summary>
    [SerializedField(18, 8)] private bool _reverseDirection = mixEffect.TransitionSettings.Wipe.ReverseDirection;

    /// <summary>
    /// Whether flip flop mode is enabled
    /// </summary>
    [SerializedField(19, 9)] private bool _flipFlop = mixEffect.TransitionSettings.Wipe.FlipFlop;

    private void SerializeInternal(byte[] buffer)
    {
        buffer.WriteUInt16BigEndian((ushort)Flag, 0);
    }
}
