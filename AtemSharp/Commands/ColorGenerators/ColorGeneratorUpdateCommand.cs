using AtemSharp.State;

namespace AtemSharp.Commands.ColorGenerators;

[Command("ColV")]
public partial class ColorGeneratorUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private byte _id;

    [DeserializedField(2)]
    [ScalingFactor(10.0)]
    private double _hue;

    [DeserializedField(4)]
    [ScalingFactor(10.0)]
    private double _saturation;

    [DeserializedField(6)]
    [ScalingFactor(10.0)]
    private double _luma;

    public void ApplyToState(AtemState state)
    {
        var colorGeneratorState = state.ColorGenerators.GetOrCreate(Id);
        colorGeneratorState.Hue = Hue;
        colorGeneratorState.Saturation = Saturation;
        colorGeneratorState.Luma = Luma;
    }
}
