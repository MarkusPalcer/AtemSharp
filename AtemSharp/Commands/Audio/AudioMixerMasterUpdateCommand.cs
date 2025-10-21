using AtemSharp.Enums;
using AtemSharp.Helpers;
using AtemSharp.Lib;
using AtemSharp.State;
using AtemSharp.State.Audio.ClassicAudio;

namespace AtemSharp.Commands.Audio;

/// <summary>
/// Update command for audio mixer master properties
/// </summary>
[Command("AMMO")]
public partial class AudioMixerMasterUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Audio gain in decibels
    /// </summary>
    public double Gain { get; internal set; }

    /// <summary>
    /// Audio balance (-50.0 to +50.0)
    /// </summary>
    public double Balance { get; internal set; }

    /// <summary>
    /// Whether audio follows fade to black
    /// </summary>
    [DeserializedField(4)]
    private bool _followFadeToBlack;

    private void DeserializeInternal(ReadOnlySpan<byte> rawCommand, ProtocolVersion _)
    {
        Gain = rawCommand.ReadUInt16BigEndian(0).UInt16ToDecibel();
        Balance = rawCommand.ReadInt16BigEndian(2).Int16ToBalance();
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var audio = state.GetClassicAudio();

        audio.Master ??= new ClassicAudioMasterChannel();
        audio.Master.Gain = Gain;
        audio.Master.Balance = Balance;
        audio.Master.FollowFadeToBlack = FollowFadeToBlack;
    }
}
