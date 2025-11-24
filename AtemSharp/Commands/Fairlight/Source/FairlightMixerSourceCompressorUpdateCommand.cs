using AtemSharp.State;

namespace AtemSharp.Commands.Fairlight.Source;

[Command("AICP")]
public partial class FairlightMixerSourceCompressorUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private ushort _inputId;

    [DeserializedField(8)]
    private long _sourceId;

    [DeserializedField(36)]
    [SerializedType(typeof(int))]
    [ScalingFactor(100)]
    private double _release;

    [DeserializedField(32)]
    [SerializedType(typeof(int))]
    [ScalingFactor(100)]
    private double _hold;

    [DeserializedField(28)]
    [SerializedType(typeof(int))]
    [ScalingFactor(100)]
    private double _attack;

    [DeserializedField(24)]
    [SerializedType(typeof(short))]
    [ScalingFactor(100)]
    private double _ratio;

    [DeserializedField(20)]
    [SerializedType(typeof(int))]
    [ScalingFactor(100)]
    private double _threshold;

    [DeserializedField(16)]
    private bool _compressorEnabled;

    public void ApplyToState(AtemState state)
    {
        var audio = state.GetFairlight();

        if (!audio.Inputs.TryGetValue(InputId, out var input))
        {
            throw new IndexOutOfRangeException($"Input ID {InputId} does not exist");
        }

        var source = input.Sources.GetOrCreate(SourceId);
        source.Id = SourceId;
        source.InputId = InputId;

        var compressor = source.Dynamics.Compressor;
        compressor.Enabled = CompressorEnabled;
        compressor.Threshold = Threshold;
        compressor.Ratio = Ratio;
        compressor.Attack = Attack;
        compressor.Hold = Hold;
        compressor.Release = Release;
    }
}
