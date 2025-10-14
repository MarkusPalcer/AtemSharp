using System.Text;
using AtemSharp.Enums;
using AtemSharp.Enums.Audio;
using AtemSharp.Enums.Ports;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Audio;


/// <summary>
/// Update command for audio mixer input properties (V8+ protocol version)
/// </summary>
[Command("AMIP", ProtocolVersion.V8_0)]
public class AudioMixerInputUpdateV8Command : IDeserializedCommand
{
    /// <summary>
    /// Audio input index
    /// </summary>
    public ushort Index { get; set; }

    /// <summary>
    /// Audio source type (readonly)
    /// </summary>
    public AudioSourceType SourceType { get; set; }

    /// <summary>
    /// External port type
    /// </summary>
    public ExternalPortType PortType { get; set; }

    /// <summary>
    /// Audio mix option
    /// </summary>
    public AudioMixOption MixOption { get; set; }

    /// <summary>
    /// Gain in decibel, -Infinity to +6dB
    /// </summary>
    public double Gain { get; set; }

    /// <summary>
    /// Balance, -50 to +50
    /// </summary>
    public double Balance { get; set; }

    /// <summary>
    /// Whether this channel supports RCA to XLR enabled setting (readonly)
    /// </summary>
    public bool SupportsRcaToXlrEnabled { get; set; }

    /// <summary>
    /// RCA to XLR enabled
    /// </summary>
    public bool RcaToXlrEnabled { get; set; }

    public static AudioMixerInputUpdateV8Command Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);

        var index = reader.ReadUInt16BigEndian();
        var sourceType = (AudioSourceType)reader.ReadByte();
        reader.ReadBytes(3); // Skip 3 bytes padding
        var portType = (ExternalPortType)reader.ReadUInt16BigEndian();
        var mixOption = (AudioMixOption)reader.ReadByte();
        reader.ReadByte(); // Skip 1 byte padding
        var gain = reader.ReadUInt16BigEndian().UInt16ToDecibel();
        var balance = reader.ReadInt16BigEndian().Int16ToBalance();
        var supportsRcaToXlrEnabled = reader.ReadBoolean();
        var rcaToXlrEnabled = reader.ReadBoolean();

        return new AudioMixerInputUpdateV8Command
        {
            Index = index,
            SourceType = sourceType,
            PortType = portType,
            MixOption = mixOption,
            Gain = gain,
            Balance = balance,
            SupportsRcaToXlrEnabled = supportsRcaToXlrEnabled,
            RcaToXlrEnabled = rcaToXlrEnabled
        };
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        if (state.Audio is null)
        {
            throw new InvalidIdError("Classic Audio", Index);
        }

        state.Audio.Channels[Index] = new ClassicAudioChannel
        {
            SourceType = SourceType,
            PortType = PortType,
            MixOption = MixOption,
            Gain = Gain,
            Balance = Balance,
            SupportsRcaToXlrEnabled = SupportsRcaToXlrEnabled,
            RcaToXlrEnabled = RcaToXlrEnabled
        };
    }
}
