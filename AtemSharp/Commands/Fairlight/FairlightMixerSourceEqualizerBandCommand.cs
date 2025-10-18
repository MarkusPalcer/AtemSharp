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
    private bool _enabled = band.Enabled;
    private byte _shape = band.Shape;
    private byte _frequencyRange = band.FrequencyRange;
    private uint _frequency = band.Frequency;
    private double _gain = band.Gain;
    private double _qFactor = band.QFactor;

    public bool Enabled
    {
        get => _enabled;
        set
        {
            _enabled = value;
            Flag |= 1 << 0;
        }
    }

    public byte Shape
    {
        get => _shape;
        set
        {
            _shape = value;
            Flag |= 1 << 1;
        }
    }

    public byte FrequencyRange
    {
        get => _frequencyRange;
        set
        {
            _frequencyRange = value;
            Flag |= 1 << 2;
        }
    }

    public uint Frequency
    {
        get => _frequency;
        set
        {
            _frequency = value;
            Flag |= 1 << 3;
        }
    }

    public double Gain
    {
        get => _gain;
        set
        {
            _gain = value;
            Flag |= 1 << 4;
        }
    }

    public double QFactor
    {
        get => _qFactor;
        set
        {
            _qFactor = value;
            Flag |= 1 << 5;
        }
    }

    public override byte[] Serialize(ProtocolVersion version)
    {
        var buffer = new byte[32];
        buffer.WriteUInt8((byte)Flag, 0);
        buffer.WriteUInt16BigEndian(_inputId, 2);
        buffer.WriteInt64BigEndian(_sourceId, 8);
        buffer.WriteUInt8(_bandIndex, 16);
        buffer.WriteBoolean(_enabled, 17);
        buffer.WriteUInt8(_shape, 18);
        buffer.WriteUInt8(_frequencyRange, 19);
        buffer.WriteUInt32BigEndian(_frequency, 20);
        buffer.WriteInt32BigEndian((int)(_gain * 100), 24);
        buffer.WriteInt16BigEndian((short)(_qFactor * 100), 28);
        return buffer;
    }
}
