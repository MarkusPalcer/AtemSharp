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

    [SerializedField(4, 0)] private AudioMixOption _mixOption = audioChannel.MixOption;

    [SerializedField(6, 1)] [CustomScaling($"{nameof(SerializationExtensions)}.{nameof(SerializationExtensions.DecibelToUInt16)}")]
    private double _gain = audioChannel.Gain;

    [SerializedField(8, 2)]
    [CustomScaling($"{nameof(SerializationExtensions)}.{nameof(SerializationExtensions.BalanceToInt16)}")]
    [SerializedType(typeof(short))]
    private double _balance = audioChannel.Balance;

    [SerializedField(10, 3)] private bool _rcaToXlrEnabled = audioChannel.RcaToXlrEnabled;
}
