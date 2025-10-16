using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;
using AtemSharp.State.Audio.ClassicAudio;

namespace AtemSharp.Commands.Audio;

/// <summary>
/// Update command for audio mixer master properties
/// </summary>
[Command("AMMO")]
public class AudioMixerMasterUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Audio gain in decibels
    /// </summary>
    public double Gain { get; init; }

    /// <summary>
    /// Audio balance (-50.0 to +50.0)
    /// </summary>
    public double Balance { get; init; }

    /// <summary>
    /// Whether audio follows fade to black
    /// </summary>
    public bool FollowFadeToBlack { get; init; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <returns>Deserialized command instance</returns>
    public static AudioMixerMasterUpdateCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new AudioMixerMasterUpdateCommand
        {
            Gain = rawCommand.ReadUInt16BigEndian(0).UInt16ToDecibel(),
            Balance = rawCommand.ReadInt16BigEndian(2).Int16ToBalance(),
            FollowFadeToBlack = rawCommand.ReadBoolean(4)
        };
    }

    /// <summary>
    /// Apply the command's values to the ATEM state
    /// </summary>
    /// <param name="state">Current ATEM state to update</param>
    /// <returns>Path indicating what was changed in the state</returns>
    /// <exception cref="InvalidIdError">Thrown if classic audio is not available</exception>
    public void ApplyToState(AtemState state)
    {
        var audio = state.GetClassicAudio();

        audio.Master ??= new ClassicAudioMasterChannel();
        audio.Master.Gain = Gain;
        audio.Master.Balance = Balance;
        audio.Master.FollowFadeToBlack = FollowFadeToBlack;
    }
}
