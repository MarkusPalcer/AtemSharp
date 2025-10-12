using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command received from ATEM device containing transition mix settings update
/// </summary>
[Command("TMxP")]
public class TransitionMixUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Mix effect index (0-based)
    /// </summary>
    public int MixEffectId { get; set; }

    /// <summary>
    /// Rate of the mix transition in frames (0-250)
    /// </summary>
    public int Rate { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <param name="protocolVersion">Protocol version used for deserialization</param>
    /// <returns>Deserialized command instance</returns>
    public static TransitionMixUpdateCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, System.Text.Encoding.Default, leaveOpen: true);

        var mixEffectId = reader.ReadByte();
        var rate = reader.ReadByte();

        return new TransitionMixUpdateCommand
        {
            MixEffectId = mixEffectId,
            Rate = rate
        };
    }

    /// <inheritdoc />
    public string[] ApplyToState(AtemState state)
    {
        // Validate mix effect index
        if (!state.Video.MixEffects.TryGetValue(MixEffectId, out var mixEffect))
        {
            throw new InvalidIdError("MixEffect", MixEffectId.ToString());
        }

        // Initialize transition settings if not present
        if (mixEffect.TransitionSettings == null)
        {
            mixEffect.TransitionSettings = new TransitionSettings();
        }

        // Initialize mix settings if not present
        if (mixEffect.TransitionSettings.Mix == null)
        {
            mixEffect.TransitionSettings.Mix = new MixTransitionSettings();
        }

        // Update the mix rate
        mixEffect.TransitionSettings.Mix.Rate = Rate;

        return new[] { $"video.mixEffects.{MixEffectId}.transitionSettings.mix.rate" };
    }
}