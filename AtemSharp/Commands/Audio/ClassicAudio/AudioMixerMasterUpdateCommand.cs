using AtemSharp.State;

namespace AtemSharp.Commands.Audio.ClassicAudio;

[Command("AMMO")]
internal partial class AudioMixerMasterUpdateCommand : IDeserializedCommand
{
    [DeserializedField(4)] private bool _followFadeToBlack;

    [DeserializedField(0)] [CustomScaling($"{nameof(DeserializationExtensions)}.{nameof(DeserializationExtensions.UInt16ToDecibel)}")]
    private double _gain;

    [DeserializedField(2)]
    [CustomScaling($"{nameof(DeserializationExtensions)}.{nameof(DeserializationExtensions.Int16ToBalance)}")]
    [SerializedType(typeof(short))]
    private double _balance;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var audio = state.GetClassicAudio();

        audio.Master.Gain = Gain;
        audio.Master.Balance = Balance;
        audio.Master.FollowFadeToBlack = FollowFadeToBlack;
    }
}
