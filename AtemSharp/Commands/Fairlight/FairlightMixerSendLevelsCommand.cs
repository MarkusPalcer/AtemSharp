using AtemSharp.Helpers;

namespace AtemSharp.Commands.Fairlight;

[Command("SFLN")]
[BufferSize(4)]
public partial class FairlightMixerSendLevelsCommand : SerializedCommand
{
    [SerializedField(0)] private bool _sendLevels;
}
