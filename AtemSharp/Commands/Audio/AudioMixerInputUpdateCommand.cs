using AtemSharp.Enums;
using AtemSharp.Enums.Audio;
using AtemSharp.Enums.Ports;
using AtemSharp.Lib;
using AtemSharp.State;
using AtemSharp.State.Audio.ClassicAudio;

namespace AtemSharp.Commands.Audio;

/// <summary>
/// Update command for audio mixer input properties
/// </summary>
[Command("AMIP")]
public class AudioMixerInputUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Audio input index
    /// </summary>
    public ushort Index { get; init; }

    /// <summary>
    /// Audio source type (readonly)
    /// </summary>
    public AudioSourceType SourceType { get; init; }

    /// <summary>
    /// External port type
    /// </summary>
    public ExternalPortType PortType { get; init; }

    /// <summary>
    /// Audio mix option
    /// </summary>
    public AudioMixOption MixOption { get; init; }

    /// <summary>
    /// Gain in decibel, -Infinity to +6dB
    /// </summary>
    public double Gain { get; init; }

    /// <summary>
    /// Balance, -50 to +50
    /// </summary>
    public double Balance { get; init; }

    public static AudioMixerInputUpdateCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new AudioMixerInputUpdateCommand
        {
            Index = rawCommand.ReadUInt16BigEndian(0),
            SourceType = (AudioSourceType)rawCommand.ReadUInt8(2),
            PortType = (ExternalPortType)rawCommand.ReadUInt16BigEndian(6),
            MixOption = (AudioMixOption)rawCommand.ReadUInt8(8),
            Gain = rawCommand.ReadUInt16BigEndian(10).UInt16ToDecibel(),
            Balance = rawCommand.ReadInt16BigEndian(12).Int16ToBalance()
        };
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        if (state.Audio is not ClassicAudioState audio)
        {
            throw new InvalidOperationException("Cannot apply AudioMixerInputUpdateCommand to non-classic audio state");
        }

        audio.Channels[Index] = new ClassicAudioChannel
        {
            SourceType = SourceType,
            PortType = PortType,
            MixOption = MixOption,
            Gain = Gain,
            Balance = Balance,
            RcaToXlrEnabled = false,
            SupportsRcaToXlrEnabled = false
        };
    }
}
