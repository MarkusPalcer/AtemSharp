using AtemSharp.Helpers;
using AtemSharp.State;

namespace AtemSharp.Commands.ColorGenerators;

[Command("ColV")]
public partial class ColorGeneratorUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private byte _id;

    [DeserializedField(2)]
    private double _hue;

    [DeserializedField(4)]
    private double _saturation;

    [DeserializedField(6)]
    private double _luma;


    // public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> data, ProtocolVersion version)
    // {
    //     return new ColorGeneratorUpdateCommand
    //     {
    //         Id = data.ReadUInt8(0),
    //         Hue = data.ReadUInt16BigEndian(2) / 10.0,
    //         Saturation = data.ReadUInt16BigEndian(4) / 10.0,
    //         Luma = data.ReadUInt16BigEndian(6) / 10.0,
    //     };
    // }

    public void ApplyToState(AtemState state)
    {
        // var colorGeneratorState = state.ColorGenerators.GetOrCreate(Id);
        // colorGeneratorState.Hue = Hue;
        // colorGeneratorState.Saturation = Saturation;
        // colorGeneratorState.Luma = Luma;
    }
}
