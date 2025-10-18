using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

[Command("CMBP")]
public class FairlightMixerMasterEqualizerBandCommand(MasterEqualizerBand band) : SerializedCommand
{

    private byte _bandIndex = band.Index;
    public BandParameter Parameters { get; } = new(band);

    public override byte[] Serialize(ProtocolVersion version)
    {
        // For testability: If flag has been set from outside, use that instead of the internal one
        if (Flag != 0) Parameters.Flag = (byte)Flag;

        var buffer = new byte[20];
        buffer.WriteUInt8(Parameters.Flag, 0);
        buffer.WriteUInt8(_bandIndex, 1);
        buffer.WriteBoolean(Parameters.Enabled, 2);
        buffer.WriteUInt8(Parameters.Shape, 3);
        buffer.WriteUInt8(Parameters.FrequencyRange, 4);
        buffer.WriteUInt32BigEndian(Parameters.Frequency, 8);
        buffer.WriteInt32BigEndian((int)(Parameters.Gain * 100), 12);
        buffer.WriteInt16BigEndian((short)(Parameters.QFactor * 100), 16);
        return buffer;
    }
}
