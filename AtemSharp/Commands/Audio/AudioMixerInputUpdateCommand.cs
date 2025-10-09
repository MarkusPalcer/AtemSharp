using System.Text;
using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Audio;

/// <summary>
/// Update command for audio mixer input properties
/// </summary>
[Command("AMIP")]
public class AudioMixerInputUpdateCommand : DeserializedCommand
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

    public static AudioMixerInputUpdateCommand Deserialize(Stream stream)
    {
        using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);

        var index = SerializationExtensions.ReadUInt16(reader);
        var sourceType = (AudioSourceType)reader.ReadByte();
        reader.ReadBytes(3); // Skip 3 bytes padding
        var portType = (ExternalPortType)SerializationExtensions.ReadUInt16(reader);
        var mixOption = (AudioMixOption)reader.ReadByte();
        reader.ReadByte(); // Skip 1 byte padding
        var gain = AtemUtil.UInt16ToDecibel(SerializationExtensions.ReadUInt16(reader));
        var balance = AtemUtil.Int16ToBalance(SerializationExtensions.ReadInt16(reader));

        return new AudioMixerInputUpdateCommand
        {
            Index = index,
            SourceType = sourceType,
            PortType = portType,
            MixOption = mixOption,
            Gain = gain,
            Balance = balance
        };
    }

    /// <inheritdoc />
    public override string[] ApplyToState(AtemState state)
    {
        if (state.Audio == null)
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
            RcaToXlrEnabled = false,
            SupportsRcaToXlrEnabled = false
        };

        return [$"audio.channels.{Index}"];
    }
}
