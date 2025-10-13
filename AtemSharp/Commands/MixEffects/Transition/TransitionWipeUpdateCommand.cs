using AtemSharp.Enums;
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
    public int MixEffectId { get; set; }

    /// <summary>
    /// Rate of the wipe transition in frames
    /// </summary>
    public int Rate { get; set; }

    /// <summary>
    /// Pattern for the wipe transition
    /// </summary>
    public int Pattern { get; set; }

    /// <summary>
    /// Width of the wipe border as percentage (0-100%)
    /// </summary>
    public double BorderWidth { get; set; }

    /// <summary>
    /// Input source for the wipe border
    /// </summary>
    public int BorderInput { get; set; }

    /// <summary>
    /// Symmetry setting for the wipe transition as percentage (0-100%)
    /// </summary>
    public double Symmetry { get; set; }

    /// <summary>
    /// Softness of the wipe border as percentage (0-100%)
    /// </summary>
    public double BorderSoftness { get; set; }

    /// <summary>
    /// X position for the wipe transition (0.0-1.0)
    /// </summary>
    public double XPosition { get; set; }

    /// <summary>
    /// Y position for the wipe transition (0.0-1.0)
    /// </summary>
    public double YPosition { get; set; }

    /// <summary>
    /// Whether the wipe direction is reversed
    /// </summary>
    public bool ReverseDirection { get; set; }

    /// <summary>
    /// Whether flip flop mode is enabled
    /// </summary>
    public bool FlipFlop { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <param name="protocolVersion">Protocol version used for deserialization</param>
    /// <returns>Deserialized command instance</returns>
    public static TransitionWipeUpdateCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, System.Text.Encoding.Default, leaveOpen: true);

        var mixEffectId = reader.ReadByte();
        var rate = reader.ReadByte();
        var pattern = reader.ReadByte();
        reader.ReadByte(); // Skip 1 byte padding (offset 3)
        var borderWidth = reader.ReadUInt16BigEndian() / 100.0;    // Convert from percentage * 100 to double
        var borderInput = reader.ReadUInt16BigEndian();
        var symmetry = reader.ReadUInt16BigEndian() / 100.0;       // Convert from percentage * 100 to double
        var borderSoftness = reader.ReadUInt16BigEndian() / 100.0; // Convert from percentage * 100 to double
        var xPosition = reader.ReadUInt16BigEndian() / 10000.0;    // Convert from 0-1 * 10000 to double
        var yPosition = reader.ReadUInt16BigEndian() / 10000.0;    // Convert from 0-1 * 10000 to double
        var reverseDirection = reader.ReadBoolean();;
        var flipFlop = reader.ReadBoolean();;

        return new TransitionWipeUpdateCommand
        {
            MixEffectId = mixEffectId,
            Rate = rate,
            Pattern = pattern,
            BorderWidth = borderWidth,
            BorderInput = borderInput,
            Symmetry = symmetry,
            BorderSoftness = borderSoftness,
            XPosition = xPosition,
            YPosition = yPosition,
            ReverseDirection = reverseDirection,
            FlipFlop = flipFlop
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

        // Initialize wipe settings if not present
        if (mixEffect.TransitionSettings.Wipe == null)
        {
            mixEffect.TransitionSettings.Wipe = new WipeTransitionSettings();
        }

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

        return new[] { $"video.mixEffects.{MixEffectId}.transitionSettings.wipe" };
    }
}