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
    /// Whether audio follows fade to black
    /// </summary>
    [DeserializedField(4)]
    private bool _followFadeToBlack;

    /// <summary>
    /// Audio gain in decibels
    /// </summary>
    [DeserializedField(0)]
    [CustomScaling($"{nameof(AtemUtil)}.{nameof(AtemUtil.UInt16ToDecibel)}")]
    private double _gain;

    /// <summary>
    /// Audio balance
    /// </summary>
    [DeserializedField(2)]
    [CustomScaling($"{nameof(AtemUtil)}.{nameof(AtemUtil.Int16ToBalance)}")]
    [SerializedType(typeof(short))]
    private double _balance;

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
