using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight.Master;

[Command("CMPP")]
[BufferSize(4)]
public partial class FairlightMixerMasterPropertiesCommand(MasterProperties master) : SerializedCommand
{
    [SerializedField(1, 0)]
    private bool _audioFollowsVideo = master.AudioFollowsVideo;
}
