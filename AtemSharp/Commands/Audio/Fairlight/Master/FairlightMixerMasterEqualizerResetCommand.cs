namespace AtemSharp.Commands.Audio.Fairlight.Master;

/// <summary>
/// Used to reset the propertries of one equalizer band on the master channel of the fairlight mixer
/// </summary>
[Command("RMOE")]
[BufferSize(4)]
public partial class FairlightMixerMasterEqualizerResetCommand : SerializedCommand
{
    [SerializedField(1,0)] private bool _equalizer;
    [SerializedField(2, 1)] private byte _band;
}
