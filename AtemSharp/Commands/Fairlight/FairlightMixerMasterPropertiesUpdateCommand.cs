using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Fairlight;

[Command("FMPP")]
public class FairlightMixerMasterPropertiesUpdateCommand : IDeserializedCommand
{
    public bool AudioFollowsVideo { get; set; }

    public static IDeserializedCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new FairlightMixerMasterPropertiesUpdateCommand
        {
            AudioFollowsVideo = rawCommand.ReadBoolean(0)
        };
    }

    public void ApplyToState(AtemState state)
    {
        if (state.Audio is not FairlightAudioState audio)
        {
            throw new InvalidOperationException("Fairlight audio state is not available");
        }

        audio.Master.AudioFollowsVideo = AudioFollowsVideo;
    }
}
