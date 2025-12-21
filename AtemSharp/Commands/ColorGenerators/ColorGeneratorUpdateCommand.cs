using AtemSharp.State;
using AtemSharp.Types;

namespace AtemSharp.Commands.ColorGenerators;

[Command("ColV")]
public partial class ColorGeneratorUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// The ID of the updated color generator
    /// </summary>
    [DeserializedField(0)] private byte _id;

    /// <summary>
    /// Gets or sets the hue
    /// </summary>
    /// <remarks>
    /// Range: [0,360[
    /// </remarks>
    [DeserializedField(2)] [ScalingFactor(10.0)]
    private double _hue;

    /// <summary>
    /// Gets or sets saturation
    /// </summary>
    /// <remarks>
    /// Range: [0.0,1.0]
    /// </remarks>
    [DeserializedField(4)] [ScalingFactor(10.0)]
    private double _saturation;

    /// <summary>
    /// Gets or sets the luminance
    /// </summary>
    /// <remarks>
    /// Range: [0.0,1.0]
    /// </remarks>
    [DeserializedField(6)] [ScalingFactor(10.0)]
    private double _luma;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var colorGeneratorState = state.ColorGenerators.GetOrCreate(Id);
        colorGeneratorState.Color = new HslColor(Hue, Saturation, Luma);
    }
}
