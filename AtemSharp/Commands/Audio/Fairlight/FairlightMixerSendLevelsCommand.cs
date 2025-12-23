namespace AtemSharp.Commands.Audio.Fairlight;

/// <summary>
/// Used to tell the ATEM device whether to send the levels of the channels
/// </summary>
[Command("SFLN")]
[BufferSize(4)]
public partial class FairlightMixerSendLevelsCommand : SerializedCommand
{
    [SerializedField(0)] private bool _sendLevels;
}
