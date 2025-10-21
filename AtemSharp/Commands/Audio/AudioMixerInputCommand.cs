using AtemSharp.Enums.Audio;
using AtemSharp.Helpers;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Audio;

/// <summary>
/// Command to update audio mixer input properties
/// </summary>
[Command("CAMI")]
[BufferSize(12)]
public partial class AudioMixerInputCommand : SerializedCommand
{
    [SerializedField(2)]
    [NoProperty]
    private readonly ushort _index;

    /// <summary>
    /// Audio mix option (Off, On, AfterFader)
    /// </summary>
    [SerializedField(4, 0)]
    private AudioMixOption _mixOption;

    /// <summary>
    /// Audio gain in decibels (-60.0 to +6.0)
    /// </summary>
    [SerializedField(6, 1)]
    [CustomScaling($"{nameof(AtemUtil)}.{nameof(AtemUtil.DecibelToUInt16)}")]
    [ValidationMethod(nameof(ValidateGain))]
    private double _gain;

    /// <summary>
    /// Audio balance (-50.0 to +50.0, where 0 is center)
    /// </summary>
    [SerializedField(8,2)]
    [CustomScaling($"{nameof(AtemUtil)}.{nameof(AtemUtil.BalanceToInt16)}")]
    [SerializedType(typeof(short))]
    [ValidationMethod(nameof(ValidateBalance))]
    private double _balance;

    /// <summary>
    /// Whether RCA to XLR conversion is enabled
    /// </summary>
    [SerializedField(10, 3)]
    private bool _rcaToXlrEnabled;

    private void ValidateGain(double value)
    {
        if (value < -60.0 || value > 6.0)
            throw new ArgumentOutOfRangeException(nameof(value), "Gain must be between -60.0 and +6.0 decibels");
    }

    private void ValidateBalance(double value)
    {
        if (value < -50.0 || value > 50.0)
            throw new ArgumentOutOfRangeException(nameof(value), "Balance must be between -50.0 and +50.0");
    }

    public AudioMixerInputCommand(ushort index, AtemState currentState)
    {
        _index = index;

        var audio = currentState.GetClassicAudio();

        // Validate audio input exists (like TypeScript update command)
        if (!audio.Channels.TryGetValue(index, out var audioChannel))
        {
            throw new IndexOutOfRangeException("Audio input with index {index} does not exist");
        }

        // Initialize from current state (direct field access = no flags)
        _mixOption = audioChannel.MixOption;
        _gain = audioChannel.Gain;
        _balance = audioChannel.Balance;
        _rcaToXlrEnabled = audioChannel.RcaToXlrEnabled;
    }
}
