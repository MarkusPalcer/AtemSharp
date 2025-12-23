using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Audio.Fairlight.Master;

/// <summary>
/// Used to set whether audio follows video for the master channel of the fairlight mixer
/// </summary>
[Command("CMPP")]
[BufferSize(4)]
public partial class FairlightMixerMasterPropertiesCommand(MasterProperties master) : SerializedCommand
{
    [SerializedField(1, 0)]
    private bool _audioFollowsVideo = master.AudioFollowsVideo;
}
