using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

[Command("CEBP")]
public class FairlightMixerSourceEqualizerBandCommand(SourceEqualizerBand band) : SerializedCommand
{
    private ushort _inputId = band.InputId;
    private long _sourceId = band.SourceId;
    private byte _bandIndex = band.Index;

    public BandParameter Parameters { get; } = new(band);

    public override byte[] Serialize(ProtocolVersion version)
    {
        // For testability: If flag has been set from outside, use that instead of the internal one
        if (Flag != 0) Parameters.Flag = (byte)Flag;

        var buffer = new byte[32];
        buffer.WriteUInt8(Parameters.Flag, 0);
        buffer.WriteUInt16BigEndian(_inputId, 2);
        buffer.WriteInt64BigEndian(_sourceId, 8);
        buffer.WriteUInt8(_bandIndex, 16);
        buffer.WriteBoolean(Parameters.Enabled, 17);
        buffer.WriteUInt8(Parameters.Shape, 18);
        buffer.WriteUInt8(Parameters.FrequencyRange, 19);
        buffer.WriteUInt32BigEndian(Parameters.Frequency, 20);
        buffer.WriteInt32BigEndian((int)(Parameters.Gain * 100), 24);
        buffer.WriteInt16BigEndian((short)(Parameters.QFactor * 100), 28);
        return buffer;
    }
}
