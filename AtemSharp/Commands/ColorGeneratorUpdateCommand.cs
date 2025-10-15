using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands;

[Command("ColV")]
public class ColorGeneratorUpdateCommand : IDeserializedCommand
{
    public int Id { get; private set; }
    public double Hue { get; private set; }
    public double Saturation { get; private set; }
    public double Luma { get; private set; }

    public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> data, ProtocolVersion version)
    {
        return new ColorGeneratorUpdateCommand
        {
            Id = data.ReadUInt8(0),
            Hue = data.ReadUInt16BigEndian(2) / 10.0,
            Saturation = data.ReadUInt16BigEndian(4) / 10.0,
            Luma = data.ReadUInt16BigEndian(6) / 10.0,
        };
    }

    public void ApplyToState(AtemState state)
    {
        var colorGeneratorState = state.ColorGenerators.GetOrCreate(Id);
        colorGeneratorState.Hue = Hue;
        colorGeneratorState.Saturation = Saturation;
        colorGeneratorState.Luma = Luma;
    }
}
