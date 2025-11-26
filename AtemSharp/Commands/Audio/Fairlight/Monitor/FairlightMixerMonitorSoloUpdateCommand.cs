using AtemSharp.State;

namespace AtemSharp.Commands.Audio.Fairlight.Monitor;

[Command("FAMS")]
public partial class FairlightMixerMonitorSoloUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private bool _solo;
    [DeserializedField(8)] private ushort _index;
    [DeserializedField(16)] private long _source;

    public void ApplyToState(AtemState state)
    {
        var solo = state.GetFairlight().Solo;
        solo.Solo = _solo;
        solo.Index = _index;
        solo.Source = _source;
    }
}
