using System.Text;
using AtemSharp.Lib;
using AtemSharp.State;

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
    public double Gain { get; set; }

    /// <summary>
    /// Audio balance (-50.0 to +50.0)
    /// </summary>
    public double Balance { get; set; }

    /// <summary>
    /// Whether audio follows fade to black
    /// </summary>
    public bool FollowFadeToBlack { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <returns>Deserialized command instance</returns>
    public static AudioMixerMasterUpdateCommand Deserialize(Stream stream)
    {
        using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);

        var gain = AtemUtil.UInt16ToDecibel(SerializationExtensions.ReadUInt16(reader));
        var balance = AtemUtil.Int16ToBalance(SerializationExtensions.ReadInt16(reader));
        var followFadeToBlack = reader.ReadBoolean();

        return new AudioMixerMasterUpdateCommand
        {
            Gain = gain,
            Balance = balance,
            FollowFadeToBlack = followFadeToBlack
        };
    }

    /// <summary>
    /// Apply the command's values to the ATEM state
    /// </summary>
    /// <param name="state">Current ATEM state to update</param>
    /// <returns>Path indicating what was changed in the state</returns>
    /// <exception cref="InvalidIdError">Thrown if classic audio is not available</exception>
    public string[] ApplyToState(AtemState state)
    {
        if (state.Audio == null)
        {
            throw new InvalidIdError("Classic Audio", "master");
        }

        // Initialize master channel if it doesn't exist
        state.Audio.Master ??= new ClassicAudioMasterChannel();

        // Update properties
        state.Audio.Master.Gain = Gain;
        state.Audio.Master.Balance = Balance;
        state.Audio.Master.FollowFadeToBlack = FollowFadeToBlack;

        return ["audio.master"];
    }
}