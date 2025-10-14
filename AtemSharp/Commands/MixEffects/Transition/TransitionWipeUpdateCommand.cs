using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command received from ATEM device containing wipe transition settings update
/// </summary>
[Command("TWpP")]
public class TransitionWipeUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Mix effect index (0-based)
    /// </summary>
    public int MixEffectId { get; init; }

    /// <summary>
    /// Rate of the wipe transition in frames
    /// </summary>
    public int Rate { get; init; }

    /// <summary>
    /// Pattern for the wipe transition
    /// </summary>
    public int Pattern { get; init; }

    /// <summary>
    /// Width of the wipe border as percentage (0-100%)
    /// </summary>
    public double BorderWidth { get; init; }

    /// <summary>
    /// Input source for the wipe border
    /// </summary>
    public int BorderInput { get; init; }

    /// <summary>
    /// Symmetry setting for the wipe transition as percentage (0-100%)
    /// </summary>
    public double Symmetry { get; init; }

    /// <summary>
    /// Softness of the wipe border as percentage (0-100%)
    /// </summary>
    public double BorderSoftness { get; init; }

    /// <summary>
    /// X position for the wipe transition (0.0-1.0)
    /// </summary>
    public double XPosition { get; init; }

    /// <summary>
    /// Y position for the wipe transition (0.0-1.0)
    /// </summary>
    public double YPosition { get; init; }

    /// <summary>
    /// Whether the wipe direction is reversed
    /// </summary>
    public bool ReverseDirection { get; init; }

    /// <summary>
    /// Whether flip flop mode is enabled
    /// </summary>
    public bool FlipFlop { get; init; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    public static TransitionWipeUpdateCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new TransitionWipeUpdateCommand
        {
            MixEffectId = rawCommand.ReadUInt8(0),
            Rate = rawCommand.ReadUInt8(1),
            Pattern = rawCommand.ReadUInt8(2),
            BorderWidth = rawCommand.ReadUInt16BigEndian(4) / 100.0,
            BorderInput = rawCommand.ReadUInt16BigEndian(6),
            Symmetry = rawCommand.ReadUInt16BigEndian(8) / 100.0,
            BorderSoftness = rawCommand.ReadUInt16BigEndian(10) / 100.0,
            XPosition = rawCommand.ReadUInt16BigEndian(12) / 10000.0,
            YPosition = rawCommand.ReadUInt16BigEndian(14) / 10000.0,
            ReverseDirection = rawCommand.ReadBoolean(16),
            FlipFlop = rawCommand.ReadBoolean(17)
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

        // Initialize wipe settings if not present
        mixEffect.TransitionSettings.Wipe ??= new WipeTransitionSettings();

        // Update the wipe settings
        mixEffect.TransitionSettings.Wipe.Rate = Rate;
        mixEffect.TransitionSettings.Wipe.Pattern = Pattern;
        mixEffect.TransitionSettings.Wipe.BorderWidth = BorderWidth;
        mixEffect.TransitionSettings.Wipe.BorderInput = BorderInput;
        mixEffect.TransitionSettings.Wipe.Symmetry = Symmetry;
        mixEffect.TransitionSettings.Wipe.BorderSoftness = BorderSoftness;
        mixEffect.TransitionSettings.Wipe.XPosition = XPosition;
        mixEffect.TransitionSettings.Wipe.YPosition = YPosition;
        mixEffect.TransitionSettings.Wipe.ReverseDirection = ReverseDirection;
        mixEffect.TransitionSettings.Wipe.FlipFlop = FlipFlop;
    }
}
