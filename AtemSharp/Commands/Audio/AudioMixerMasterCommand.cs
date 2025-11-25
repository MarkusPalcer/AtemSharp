using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Audio;

/// <summary>
/// Command to update audio mixer master properties
/// </summary>
[Command("CAMM")]
[BufferSize(8)]
public partial class AudioMixerMasterCommand : SerializedCommand
{
    /// <summary>
    /// Audio gain in decibels
    /// </summary>
    [SerializedField(2,0)]
    [CustomScaling($"{nameof(SerializationExtensions)}.{nameof(SerializationExtensions.DecibelToUInt16)}")]
    private double _gain;

    /// <summary>
    /// Audio balance
    /// </summary>
    [SerializedField(4,1)]
    [CustomScaling($"{nameof(SerializationExtensions)}.{nameof(SerializationExtensions.BalanceToInt16)}")]
    [SerializedType(typeof(short))]
    private double _balance;

    /// <summary>
    /// Whether audio follows fade to black
    /// </summary>
    [SerializedField(6,2)]
    private bool _followFadeToBlack; // TODO: The original library claims that this never worked

    /// <summary>
    /// Create command initialized with current state values
    /// </summary>
    /// <param name="currentState">Current ATEM state</param>
    /// <exception cref="InvalidIdError">Thrown if classic audio not available</exception>
    public AudioMixerMasterCommand(AtemState currentState)
    {
        var audio = currentState.GetClassicAudio();

        if (audio.Master is null)
        {
            throw new InvalidOperationException("Master audio channel is not available (yet)");
        }

        var audioMaster = audio.Master;

        // Initialize from current state (direct field access = no flags set)
        _gain = audioMaster.Gain;
        _balance = audioMaster.Balance;
        _followFadeToBlack = audioMaster.FollowFadeToBlack;
    }
}
