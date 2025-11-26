namespace AtemSharp.Commands.Audio.Fairlight.Master;

[Command("RMOE")]
[BufferSize(4)]
public partial class FairlightMixerMasterEqualizerResetCommand : SerializedCommand
{
    [SerializedField(1,0)] private bool _equalizer;
    [SerializedField(2, 1)] private byte _band;
}
