using AtemSharp.State.Audio.ClassicAudio;

namespace AtemSharp.Commands.Audio.ClassicAudio;

/// <summary>
/// Command to update audio mixer input properties
/// </summary>
[Command("CAMI")]
[BufferSize(12)]
public partial class AudioMixerInputCommand(ClassicAudioChannel audioChannel) : SerializedCommand
{
    [SerializedField(2)] [NoProperty] private readonly ushort _index = audioChannel.Id;

    /// <summary>
    /// Audio mix option
    /// </summary>
    [SerializedField(4, 0)] private AudioMixOption _mixOption = audioChannel.MixOption;

    /// <summary>
    /// Audio gain in decibels
    /// </summary>
    [SerializedField(6, 1)] [CustomScaling($"{nameof(SerializationExtensions)}.{nameof(SerializationExtensions.DecibelToUInt16)}")]
    private double _gain = audioChannel.Gain;

    /// <summary>
    /// Audio balance
    /// </summary>
    [SerializedField(8, 2)]
    [CustomScaling($"{nameof(SerializationExtensions)}.{nameof(SerializationExtensions.BalanceToInt16)}")]
    [SerializedType(typeof(short))]
    private double _balance = audioChannel.Balance;

    /// <summary>
    /// Whether RCA to XLR conversion is enabled
    /// </summary>
    [SerializedField(10, 3)] private bool _rcaToXlrEnabled = audioChannel.RcaToXlrEnabled;
}
