using AtemSharp.State;

namespace AtemSharp.Commands.DownstreamKey;

/// <summary>
/// Command received from ATEM device containing downstream keyer properties
/// </summary>
[Command("DskP")]
public partial class DownstreamKeyPropertiesUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _downstreamKeyerId;

    [DeserializedField(1)] private bool _tie;

    [DeserializedField(2)] private byte _rate;

    [DeserializedField(3)] private bool _preMultiply;

    [DeserializedField(4)] [ScalingFactor(10.0)]
    private double _clip;

    [DeserializedField(6)] [ScalingFactor(10)]
    private double _gain;

    [DeserializedField(8)] private bool _invert;

    [DeserializedField(9)] private bool _maskEnabled;

    [DeserializedField(10)] [SerializedType(typeof(short))] [ScalingFactor(1000.0)]
    private double _maskTop;

    [DeserializedField(12)] [SerializedType(typeof(short))] [ScalingFactor(1000.0)]
    private double _maskBottom;

    [DeserializedField(14)] [SerializedType(typeof(short))] [ScalingFactor(1000.0)]
    private double _maskLeft;

    [DeserializedField(16)] [SerializedType(typeof(short))] [ScalingFactor(1000.0)]
    private double _maskRight;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Update the properties
        var properties = state.Video.DownstreamKeyers[DownstreamKeyerId].Properties;

        properties.Tie = _tie;
        properties.Rate = _rate;
        properties.PreMultiply = _preMultiply;
        properties.Gain = _gain;
        properties.Clip = _clip;
        properties.Invert = _invert;

        properties.Mask.Enabled = _maskEnabled;
        properties.Mask.Top = _maskTop;
        properties.Mask.Bottom = _maskBottom;
        properties.Mask.Left = _maskLeft;
        properties.Mask.Right = _maskRight;
    }
}
