using AtemSharp.State;

namespace AtemSharp.Commands.Audio.ClassicAudio;

[Command("AMmO")]
internal partial class AudioMixerMonitorUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private bool _enabled;

    [DeserializedField(2)] [CustomScaling($"{nameof(DeserializationExtensions)}.{nameof(DeserializationExtensions.UInt16ToDecibel)}")]
    private double _gain;

    [DeserializedField(4)] private bool _mute;
    [DeserializedField(5)] private bool _solo;
    [DeserializedField(6)] private ushort _soloSource;
    [DeserializedField(8)] private bool _dim;

    [DeserializedField(10)] [ScalingFactor(100.0)]
    private double _dimLevel;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var audio = state.GetClassicAudio();
        audio.Monitor.Enabled = Enabled;
        audio.Monitor.Gain = Gain;
        audio.Monitor.Mute = Mute;
        audio.Monitor.Solo = Solo;
        audio.Monitor.SoloSource = SoloSource;
        audio.Monitor.Dim = Dim;
        audio.Monitor.DimLevel = DimLevel;
    }
}
