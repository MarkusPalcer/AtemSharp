using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command received from ATEM device containing DVE transition settings update
/// </summary>
[Command("TDvP")]
// ReSharper disable once InconsistentNaming Domain Specific Acronym
public class TransitionDVEUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Mix effect index (0-based)
    /// </summary>
    public int MixEffectId { get; set; }

    /// <summary>
    /// Transition rate in frames
    /// </summary>
    public int Rate { get; set; }

    /// <summary>
    /// Logo/key transition rate in frames
    /// </summary>
    public int LogoRate { get; set; }

    /// <summary>
    /// DVE effect style
    /// </summary>
    public DVEEffect Style { get; set; }

    /// <summary>
    /// Fill source input number
    /// </summary>
    public int FillSource { get; set; }

    /// <summary>
    /// Key source input number
    /// </summary>
    public int KeySource { get; set; }

    /// <summary>
    /// Whether the key is enabled
    /// </summary>
    public bool EnableKey { get; set; }

    /// <summary>
    /// Whether the key is pre-multiplied
    /// </summary>
    public bool PreMultiplied { get; set; }

    /// <summary>
    /// Key clip value (0.0 to 100.0)
    /// </summary>
    public double Clip { get; set; }

    /// <summary>
    /// Key gain value (0.0 to 100.0)
    /// </summary>
    public double Gain { get; set; }

    /// <summary>
    /// Whether the key is inverted
    /// </summary>
    public bool InvertKey { get; set; }

    /// <summary>
    /// Whether the transition is reversed
    /// </summary>
    public bool Reverse { get; set; }

    /// <summary>
    /// Whether flip-flop is enabled
    /// </summary>
    public bool FlipFlop { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <param name="protocolVersion">Protocol version used for deserialization</param>
    /// <returns>Deserialized command instance</returns>
    public static TransitionDVEUpdateCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, System.Text.Encoding.Default, leaveOpen: true);

        var mixEffectId = reader.ReadByte();
        var rate = reader.ReadByte();
        var logoRate = reader.ReadByte();
        var style = (DVEEffect)reader.ReadByte();

        // The TypeScript code reads fillSource and keySource as two separate bytes and combines them
        // fillSource: (rawCommand.readUInt8(4) << 8) | (rawCommand.readUInt8(5) & 0xff)
        // keySource: (rawCommand.readUInt8(6) << 8) | (rawCommand.readUInt8(7) & 0xff)
        var fillSourceHigh = reader.ReadByte();
        var fillSourceLow = reader.ReadByte();
        var fillSource = (fillSourceHigh << 8) | fillSourceLow;

        var keySourceHigh = reader.ReadByte();
        var keySourceLow = reader.ReadByte();
        var keySource = (keySourceHigh << 8) | keySourceLow;

        var enableKey = reader.ReadBoolean();;
        var preMultiplied = reader.ReadBoolean();;
        var clip = reader.ReadUInt16BigEndian() / 10.0;  // Convert from fixed-point to double
        var gain = reader.ReadUInt16BigEndian() / 10.0;  // Convert from fixed-point to double
        var invertKey = reader.ReadBoolean();;
        var reverse = reader.ReadBoolean();;
        var flipFlop = reader.ReadBoolean();;

        return new TransitionDVEUpdateCommand
        {
            MixEffectId = mixEffectId,
            Rate = rate,
            LogoRate = logoRate,
            Style = style,
            FillSource = fillSource,
            KeySource = keySource,
            EnableKey = enableKey,
            PreMultiplied = preMultiplied,
            Clip = clip,
            Gain = gain,
            InvertKey = invertKey,
            Reverse = reverse,
            FlipFlop = flipFlop
        };
    }

    /// <inheritdoc />
    public string[] ApplyToState(AtemState state)
    {
        // Validate mix effect index and capabilities
        if (!state.Video.MixEffects.TryGetValue(MixEffectId, out var mixEffect))
        {
            throw new InvalidIdError("MixEffect", MixEffectId.ToString());
        }

        // Check if DVE is supported (mirroring TypeScript logic)
        if (state.Info?.Capabilities?.DVEs <= 0)
        {
            throw new InvalidIdError("DVE", "is not supported");
        }

        // Initialize transition settings if not present
        if (mixEffect.TransitionSettings == null)
        {
            mixEffect.TransitionSettings = new TransitionSettings();
        }

        // Update DVE settings (mirroring TypeScript pattern)
        mixEffect.TransitionSettings.DVE = new DVETransitionSettings
        {
            Rate = Rate,
            LogoRate = LogoRate,
            Style = Style,
            FillSource = FillSource,
            KeySource = KeySource,
            EnableKey = EnableKey,
            PreMultiplied = PreMultiplied,
            Clip = Clip,
            Gain = Gain,
            InvertKey = InvertKey,
            Reverse = Reverse,
            FlipFlop = FlipFlop
        };

        return new[] { $"video.mixEffects.{MixEffectId}.transitionSettings.DVE" };
    }
}