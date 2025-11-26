using AtemSharp.State;

namespace AtemSharp.Commands.ColorGenerators;

[Command("CClV")]
[BufferSize(8)]
public partial class ColorGeneratorCommand(ColorGeneratorState colorGenerator) : SerializedCommand
{
    [SerializedField(1, 0)] [NoProperty] private readonly byte _id = colorGenerator.Id;

    [SerializedField(2, 0)] [ScalingFactor(10.0)]
    private double _hue = colorGenerator.Hue;

    [SerializedField(4, 1)] [ScalingFactor(10.0)]
    private double _saturation = colorGenerator.Saturation;

    [SerializedField(6, 2)] [ScalingFactor(10.0)]
    private double _luma = colorGenerator.Luma;
}
