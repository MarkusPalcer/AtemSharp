using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command received from ATEM device containing stinger transition settings update
/// </summary>
[Command("TStP")]
public class TransitionStingerUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Mix effect index (0-based)
    /// </summary>
    public int MixEffectId { get; init; }

    /// <summary>
    /// Source for the stinger transition
    /// </summary>
    public int Source { get; init; }

    /// <summary>
    /// Whether the key is pre-multiplied
    /// </summary>
    public bool PreMultipliedKey { get; init; }

    /// <summary>
    /// Clip value for the stinger transition
    /// </summary>
    public double Clip { get; init; }

    /// <summary>
    /// Gain value for the stinger transition (0-100%)
    /// </summary>
    public double Gain { get; init; }

    /// <summary>
    /// Whether the stinger transition is inverted
    /// </summary>
    public bool Invert { get; init; }

    /// <summary>
    /// Preroll value for the stinger transition
    /// </summary>
    public int Preroll { get; init; }

    /// <summary>
    /// Clip duration for the stinger transition
    /// </summary>
    public int ClipDuration { get; init; }

    /// <summary>
    /// Trigger point for the stinger transition
    /// </summary>
    public int TriggerPoint { get; init; }

    /// <summary>
    /// Mix rate for the stinger transition
    /// </summary>
    public int MixRate { get; init; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    public static TransitionStingerUpdateCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        // According to TypeScript: these use bit shifting to combine bytes (big endian manual reconstruction)
        // TODO: Check if helper Method can be used instead of manual bit-shifting
        return new TransitionStingerUpdateCommand
        {
            MixEffectId = rawCommand.ReadUInt8(0),
            Source = rawCommand.ReadUInt8(1),
            PreMultipliedKey = rawCommand.ReadBoolean(2),
            Clip = rawCommand.ReadUInt16BigEndian(4) / 10.0,
            Gain = rawCommand.ReadUInt16BigEndian(6) / 10.0,
            Invert = rawCommand.ReadBoolean(8),
            Preroll = (rawCommand.ReadUInt8(10) << 8) | rawCommand.ReadUInt8(11),
            ClipDuration = (rawCommand.ReadUInt8(12) << 8) | rawCommand.ReadUInt8(13),
            TriggerPoint = (rawCommand.ReadUInt8(14) << 8) | rawCommand.ReadUInt8(15),
            MixRate = (rawCommand.ReadUInt8(16) << 8) | rawCommand.ReadUInt8(17)
        };
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Validate mix effect index
        if (!state.Video.MixEffects.TryGetValue(MixEffectId, out var mixEffect))
        {
            throw new InvalidIdError("MixEffect", MixEffectId.ToString());
        }

        // Initialize transition settings if not present
        mixEffect.TransitionSettings ??= new TransitionSettings();

        // Initialize stinger settings if not present
        mixEffect.TransitionSettings.Stinger ??= new StingerTransitionSettings();

        // Update the stinger settings
        mixEffect.TransitionSettings.Stinger.Source = Source;
        mixEffect.TransitionSettings.Stinger.PreMultipliedKey = PreMultipliedKey;
        mixEffect.TransitionSettings.Stinger.Clip = Clip;
        mixEffect.TransitionSettings.Stinger.Gain = Gain;
        mixEffect.TransitionSettings.Stinger.Invert = Invert;
        mixEffect.TransitionSettings.Stinger.Preroll = Preroll;
        mixEffect.TransitionSettings.Stinger.ClipDuration = ClipDuration;
        mixEffect.TransitionSettings.Stinger.TriggerPoint = TriggerPoint;
        mixEffect.TransitionSettings.Stinger.MixRate = MixRate;
    }
}
