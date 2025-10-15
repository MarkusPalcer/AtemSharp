using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands;

[Command("CClV")]
public class ColorGeneratorCommand : SerializedCommand
{
    private double _hue;
    private double _saturation;
    private double _luma;

    public double Hue
    {
        get => _hue;
        set { _hue = value;
            Flag |= 1 << 0;
        }
    }

    public double Saturation
    {
        get => _saturation;
        set
        {
            _saturation = value;
            Flag |= 1 << 1;
        }
    }

    public double Luma
    {
        get => _luma;
        set
        {
            _luma = value;
            Flag |= 1 << 2;
        }
    }

    public byte Id { get; }

    public ColorGeneratorCommand(AtemState state, byte id)
    {
        if (!state.ColorGenerators.TryGetValue(id, out var colorGenerator))
        {
            throw new IndexOutOfRangeException("Color generator with ID {id} does not exist");
        }

        Id = id;
        _hue = colorGenerator.Hue;
        _saturation = colorGenerator.Saturation;
        _luma = colorGenerator.Luma;
    }

    public override byte[] Serialize(ProtocolVersion version)
    {
        var buffer = new byte[8];
        buffer.WriteUInt8((byte)Flag, 0);
        buffer.WriteUInt8(Id, 1);
        buffer.WriteUInt16BigEndian(Hue * 10.0, 2);
        buffer.WriteUInt16BigEndian(Saturation * 10.0, 4);
        buffer.WriteUInt16BigEndian(Luma * 10.0, 6);

        return buffer;
    }
}
