using System.Text;
using AtemSharp.Enums;
using AtemSharp.Lib;

namespace AtemSharp.Commands3.Audio;

/// <summary>
/// Command to update audio mixer input properties using reflection-based property mapping
/// </summary>
/// <param name="index">Audio input index (0-based)</param>
[Command("CAMI")]
public class AudioMixerInputCommand(ushort index) : WritableCommand<AudioMixerInputCommand>
{
    /// <summary>
    /// Audio input index
    /// </summary>
    public ushort Index => index;

    /// <summary>
    /// Audio mix option (Off, On, AfterFader)
    /// </summary>
    [CommandProperty(1 << 0, 0)]
    public AudioMixOption? MixOption { get; set; }

    /// <summary>
    /// Audio gain in decibels (-60.0 to +6.0)
    /// </summary>
    [CommandProperty(1 << 1, 1)]
    public double? Gain { get; set; }

    /// <summary>
    /// Audio balance (-50.0 to +50.0, where 0 is center)
    /// </summary>
    [CommandProperty(1 << 2, 2)]
    public double? Balance { get; set; }

    /// <summary>
    /// Whether RCA to XLR conversion is enabled
    /// </summary>
    [CommandProperty(1 << 3, 3)]
    public bool RcaToXlrEnabled { get; set; }

    /// <summary>
    /// Serialize command to binary stream for transmission to ATEM
    /// </summary>
    /// <param name="version">Protocol version</param>
    /// <returns>Serialized command data as stream</returns>
    public override Stream Serialize(ProtocolVersion version)
    {
        var buffer = new MemoryStream();
        using var writer = new BinaryWriter(buffer, Encoding.Default, leaveOpen: true);
        
        // Write flag indicating which properties are being updated
        writer.Write(Flag);
        
        // Write audio input index (big-endian 16-bit)
        writer.WriteUInt16BE(Index);
        
        // Write properties in order based on ATEM protocol specification
        writer.Write((byte)(MixOption ?? AudioMixOption.Off));
        writer.WriteUInt16BE(AtemUtil.DecibelToUInt16BE(Gain ?? 0.0));
        writer.WriteInt16BE(AtemUtil.BalanceToInt(Balance ?? 0.0));
        writer.Write(RcaToXlrEnabled ? (byte)1 : (byte)0);

        return buffer;
    }

    /// <summary>
    /// Update specific audio mixer properties with validation
    /// </summary>
    /// <param name="mixOption">Audio mix option</param>
    /// <param name="gain">Gain in decibels (-60 to +6)</param>
    /// <param name="balance">Balance (-50 to +50)</param>
    /// <param name="rcaToXlrEnabled">RCA to XLR enabled state</param>
    /// <returns>True if any properties were updated</returns>
    public bool UpdateAudioProperties(
        AudioMixOption? mixOption = null,
        double? gain = null,
        double? balance = null,
        bool? rcaToXlrEnabled = null)
    {
        var newCommand = new AudioMixerInputCommand(Index)
        {
            MixOption = mixOption ?? MixOption,
            Gain = ValidateGain(gain) ?? Gain,
            Balance = ValidateBalance(balance) ?? Balance,
            RcaToXlrEnabled = rcaToXlrEnabled ?? RcaToXlrEnabled
        };

        return UpdateProps(newCommand);
    }

    /// <summary>
    /// Validate gain value is within acceptable range
    /// </summary>
    /// <param name="gain">Gain value to validate</param>
    /// <returns>Validated gain value</returns>
    /// <exception cref="ArgumentOutOfRangeException">If gain is outside valid range</exception>
    private static double? ValidateGain(double? gain)
    {
        if (gain.HasValue && (gain.Value < -60.0 || gain.Value > 6.0))
        {
            throw new ArgumentOutOfRangeException(nameof(gain), 
                "Gain must be between -60.0 and +6.0 decibels");
        }
        return gain;
    }

    /// <summary>
    /// Validate balance value is within acceptable range
    /// </summary>
    /// <param name="balance">Balance value to validate</param>
    /// <returns>Validated balance value</returns>
    /// <exception cref="ArgumentOutOfRangeException">If balance is outside valid range</exception>
    private static double? ValidateBalance(double? balance)
    {
        if (balance.HasValue && (balance.Value < -50.0 || balance.Value > 50.0))
        {
            throw new ArgumentOutOfRangeException(nameof(balance), 
                "Balance must be between -50.0 and +50.0");
        }
        return balance;
    }
}