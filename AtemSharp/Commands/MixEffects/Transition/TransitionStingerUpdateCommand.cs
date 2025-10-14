using AtemSharp.Enums;
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
    public int MixEffectId { get; set; }

    /// <summary>
    /// Source for the stinger transition
    /// </summary>
    public int Source { get; set; }

    /// <summary>
    /// Whether the key is pre-multiplied
    /// </summary>
    public bool PreMultipliedKey { get; set; }

    /// <summary>
    /// Clip value for the stinger transition
    /// </summary>
    public double Clip { get; set; }

    /// <summary>
    /// Gain value for the stinger transition (0-100%)
    /// </summary>
    public double Gain { get; set; }

    /// <summary>
    /// Whether the stinger transition is inverted
    /// </summary>
    public bool Invert { get; set; }

    /// <summary>
    /// Preroll value for the stinger transition
    /// </summary>
    public int Preroll { get; set; }

    /// <summary>
    /// Clip duration for the stinger transition
    /// </summary>
    public int ClipDuration { get; set; }

    /// <summary>
    /// Trigger point for the stinger transition
    /// </summary>
    public int TriggerPoint { get; set; }

    /// <summary>
    /// Mix rate for the stinger transition
    /// </summary>
    public int MixRate { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <param name="protocolVersion">Protocol version used for deserialization</param>
    /// <returns>Deserialized command instance</returns>
    public static TransitionStingerUpdateCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, System.Text.Encoding.Default, leaveOpen: true);

        var mixEffectId = reader.ReadByte();
        var source = reader.ReadByte();
        var preMultipliedKey = reader.ReadBoolean();;
        reader.ReadByte(); // Skip 1 byte padding (offset 3)

        var clip = reader.ReadUInt16BigEndian() / 10.0;    // Convert from value * 10 to double
        var gain = reader.ReadUInt16BigEndian() / 10.0;    // Convert from value * 10 to double
        var invert = reader.ReadBoolean();;
        reader.ReadByte(); // Skip 1 byte padding (offset 9)

        // According to TypeScript: these use bit shifting to combine bytes (big endian manual reconstruction)
        var preroll = (reader.ReadByte() << 8) | reader.ReadByte();
        var clipDuration = (reader.ReadByte() << 8) | reader.ReadByte();
        var triggerPoint = (reader.ReadByte() << 8) | reader.ReadByte();
        var mixRate = (reader.ReadByte() << 8) | reader.ReadByte();

        return new TransitionStingerUpdateCommand
        {
            MixEffectId = mixEffectId,
            Source = source,
            PreMultipliedKey = preMultipliedKey,
            Clip = clip,
            Gain = gain,
            Invert = invert,
            Preroll = preroll,
            ClipDuration = clipDuration,
            TriggerPoint = triggerPoint,
            MixRate = mixRate
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
        if (mixEffect.TransitionSettings == null)
        {
            mixEffect.TransitionSettings = new TransitionSettings();
        }

        // Initialize stinger settings if not present
        if (mixEffect.TransitionSettings.Stinger == null)
        {
            mixEffect.TransitionSettings.Stinger = new StingerTransitionSettings();
        }

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
