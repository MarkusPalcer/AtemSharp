using AtemSharp.Helpers;
using AtemSharp.State;

namespace AtemSharp.Commands.ColorGenerators;

[Command("CClV")]
[BufferSize(8)]
public partial class ColorGeneratorCommand : SerializedCommand
{
    [SerializedField(1, 0)]
    [NoProperty]
    private byte _id;

    [SerializedField(2,0)]
    [ScalingFactor(10.0)]
    private double _hue;

    [SerializedField(4,1)]
    [ScalingFactor(10.0)]
    private double _saturation;

    [SerializedField(6,2)]
    [ScalingFactor(10.0)]
    private double _luma;

    public ColorGeneratorCommand(AtemState state, byte id)
    {
        if (!state.ColorGenerators.TryGetValue(id, out var colorGenerator))
        {
            throw new IndexOutOfRangeException("Color generator with ID {id} does not exist");
        }

        _id = id;
        _hue = colorGenerator.Hue;
        _saturation = colorGenerator.Saturation;
        _luma = colorGenerator.Luma;
    }
}
