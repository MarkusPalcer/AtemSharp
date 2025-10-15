using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Fairlight;

[Command("CMPP")]
public class FairlightMixerMasterPropertiesCommand : SerializedCommand
{
    private bool _audioFollowsVideo;

    public bool AudioFollowsVideo
    {
        get => _audioFollowsVideo;
        set
        {
            _audioFollowsVideo = value;
            Flag |= 1 << 0;
        }
    }

    public FairlightMixerMasterPropertiesCommand(AtemState state)
    {
        if (state.Audio is not FairlightAudioState fairlight)
        {
            throw new InvalidOperationException("Fairlight audio state is not available");
        }

        _audioFollowsVideo = fairlight.Master.AudioFollowsVideo;
    }

    public override byte[] Serialize(ProtocolVersion version)
    {
        var buffer = new byte[4];
        buffer.WriteUInt8((byte)Flag, 0);
        buffer.WriteBoolean(AudioFollowsVideo, 1);

        return buffer;
    }
}
