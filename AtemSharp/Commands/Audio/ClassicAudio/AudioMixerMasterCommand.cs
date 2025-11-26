using AtemSharp.State.Audio.ClassicAudio;

namespace AtemSharp.Commands.Audio.ClassicAudio;

/// <summary>
/// Command to update audio mixer master properties
/// </summary>
[Command("CAMM")]
[BufferSize(8)]
public partial class AudioMixerMasterCommand(ClassicAudioState currentState) : SerializedCommand
{
    /// <summary>
    /// Audio gain in decibels
    /// </summary>
    [SerializedField(2, 0)] [CustomScaling($"{nameof(SerializationExtensions)}.{nameof(SerializationExtensions.DecibelToUInt16)}")]
    private double _gain = currentState.Master.Gain;

    /// <summary>
    /// Audio balance
    /// </summary>
    [SerializedField(4, 1)]
    [CustomScaling($"{nameof(SerializationExtensions)}.{nameof(SerializationExtensions.BalanceToInt16)}")]
    [SerializedType(typeof(short))]
    private double _balance = currentState.Master.Balance;

    /// <summary>
    /// Whether audio follows fade to black
    /// </summary>
    [SerializedField(6, 2)]
    private bool
        _followFadeToBlack =
            currentState.Master.FollowFadeToBlack;
}
