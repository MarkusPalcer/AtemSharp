using AtemSharp.Enums;
using AtemSharp.Lib;
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
    public int MixEffectId { get; init; }

    /// <summary>
    /// Transition rate in frames
    /// </summary>
    public int Rate { get; init; }

    /// <summary>
    /// Logo/key transition rate in frames
    /// </summary>
    public int LogoRate { get; init; }

    /// <summary>
    /// DVE effect style
    /// </summary>
    public DVEEffect Style { get; init; }

    /// <summary>
    /// Fill source input number
    /// </summary>
    public int FillSource { get; init; }

    /// <summary>
    /// Key source input number
    /// </summary>
    public int KeySource { get; init; }

    /// <summary>
    /// Whether the key is enabled
    /// </summary>
    public bool EnableKey { get; init; }

    /// <summary>
    /// Whether the key is pre-multiplied
    /// </summary>
    public bool PreMultiplied { get; init; }

    /// <summary>
    /// Key clip value (0.0 to 100.0)
    /// </summary>
    public double Clip { get; init; }

    /// <summary>
    /// Key gain value (0.0 to 100.0)
    /// </summary>
    public double Gain { get; init; }

    /// <summary>
    /// Whether the key is inverted
    /// </summary>
    public bool InvertKey { get; init; }

    /// <summary>
    /// Whether the transition is reversed
    /// </summary>
    public bool Reverse { get; init; }

    /// <summary>
    /// Whether flip-flop is enabled
    /// </summary>
    public bool FlipFlop { get; init; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    public static TransitionDVEUpdateCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        var mixEffectId = rawCommand.ReadUInt8(0);
        var rate = rawCommand.ReadUInt8(1);
        var logoRate = rawCommand.ReadUInt8(2);
        var style = (DVEEffect)rawCommand.ReadUInt8(3);

        // The TypeScript code reads fillSource and keySource as two separate bytes and combines them
        // fillSource: (rawCommand.readUInt8(4) << 8) | (rawCommand.readUInt8(5) & 0xff)
        // keySource: (rawCommand.readUInt8(6) << 8) | (rawCommand.readUInt8(7) & 0xff)
        // TODO: Use helper method for single read
        var fillSourceHigh = rawCommand.ReadUInt8(4);
        var fillSourceLow = rawCommand.ReadUInt8(5);
        var fillSource = rawCommand.ReadUInt16BigEndian(4); //(fillSourceHigh << 8) | fillSourceLow;

        // TODO: Use helper method for single read
        var keySourceHigh = rawCommand.ReadUInt8(6);
        var keySourceLow = rawCommand.ReadUInt8(7);
        var keySource = (keySourceHigh << 8) | keySourceLow;

        var enableKey = rawCommand.ReadBoolean(8);
        var preMultiplied = rawCommand.ReadBoolean(9);
        var clip = rawCommand.ReadUInt16BigEndian(10) / 10.0;  // Convert from fixed-point to double
        var gain = rawCommand.ReadUInt16BigEndian(12) / 10.0;  // Convert from fixed-point to double
        var invertKey = rawCommand.ReadBoolean(14);
        var reverse = rawCommand.ReadBoolean(15);
        var flipFlop = rawCommand.ReadBoolean(16);

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
    public void ApplyToState(AtemState state)
    {
        // Validate mix effect index and capabilities
        if (!state.Video.MixEffects.TryGetValue(MixEffectId, out var mixEffect))
        {
            throw new InvalidIdError("MixEffect", MixEffectId.ToString());
        }

        // Check if DVE is supported (mirroring TypeScript logic)
        if (state.Info.Capabilities?.DVEs <= 0)
        {
            throw new InvalidIdError("DVE", "is not supported");
        }

        // Initialize transition settings if not present
        mixEffect.TransitionSettings ??= new TransitionSettings();

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
    }
}
