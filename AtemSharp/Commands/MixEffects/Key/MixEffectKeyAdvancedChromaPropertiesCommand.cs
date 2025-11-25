using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Key;

/// <summary>
/// Command to update advanced chroma key properties for an upstream keyer
/// </summary>
[Command("CACK")]
[BufferSize(28)]
public partial class MixEffectKeyAdvancedChromaPropertiesCommand : SerializedCommand
{
    [SerializedField(2)] [NoProperty] private readonly byte _mixEffectId;

    [SerializedField(3)] [NoProperty] private readonly byte _keyerId;


    /// <summary>
    /// Foreground level value
    /// </summary>
    [SerializedField(4, 0)] [ScalingFactor(10)]
    private double _foregroundLevel;

    /// <summary>
    /// Background level value
    /// </summary>
    [SerializedField(6, 1)] [ScalingFactor(10)]
    private double _backgroundLevel;

    /// <summary>
    /// Key edge value
    /// </summary>
    [SerializedField(8, 2)] [ScalingFactor(10)]
    private double _keyEdge;

    /// <summary>
    /// Spill suppression value
    /// </summary>
    [SerializedField(10, 3)] [ScalingFactor(10)]
    private double _spillSuppression;

    /// <summary>
    /// Flare suppression value
    /// </summary>
    [SerializedField(12, 4)] [ScalingFactor(10)]
    private double _flareSuppression;

    /// <summary>
    /// Brightness adjustment value
    /// </summary>
    [SerializedField(14, 5)] [ScalingFactor(10)] [SerializedType(typeof(short))]
    private double _brightness;


    /// <summary>
    /// Contrast adjustment value
    /// </summary>
    [SerializedField(16, 6)] [ScalingFactor(10)] [SerializedType(typeof(short))]
    private double _contrast;


    /// <summary>
    /// Saturation adjustment value
    /// </summary>
    [SerializedField(18, 7)] [ScalingFactor(10)]
    private double _saturation;

    /// <summary>
    /// Red color adjustment value
    /// </summary>
    [SerializedField(20, 8)] [ScalingFactor(10)] [SerializedType(typeof(short))]
    private double _red;

    /// <summary>
    /// Green color adjustment value
    /// </summary>
    [SerializedField(22, 9)] [ScalingFactor(10)] [SerializedType(typeof(short))]
    private double _green;

    /// <summary>
    /// Blue color adjustment value
    /// </summary>
    [SerializedField(24, 10)] [ScalingFactor(10)] [SerializedType(typeof(short))]
    private double _blue;


    /// <summary>
    /// Create command initialized with current state values
    /// </summary>
    /// <param name="mixEffectId">Mix effect index (0-based)</param>
    /// <param name="keyerId">Upstream keyer index (0-based)</param>
    /// <param name="currentState">Current ATEM state</param>
    /// <exception cref="InvalidIdError">Thrown if mix effect or keyer not available</exception>
    public MixEffectKeyAdvancedChromaPropertiesCommand(byte mixEffectId, byte keyerId, AtemState currentState)
    {
        _mixEffectId = mixEffectId;
        _keyerId = keyerId;
        var mixEffect = currentState.Video.MixEffects[mixEffectId];

        // If no video state or mix effect doesn't exist, initialize with defaults
        if (!mixEffect.UpstreamKeyers.TryGetValue(keyerId, out var keyer))
        {
            // Set default values and flags (like TypeScript pattern)
            ForegroundLevel = 0.0;
            BackgroundLevel = 0.0;
            KeyEdge = 0.0;
            SpillSuppression = 0.0;
            FlareSuppression = 0.0;
            Brightness = 0.0;
            Contrast = 0.0;
            Saturation = 0.0;
            Red = 0.0;
            Green = 0.0;
            Blue = 0.0;
            return;
        }

        var properties = keyer.AdvancedChromaSettings.Properties;

        // Initialize from current state (direct field access = no flags set)
        _foregroundLevel = properties.ForegroundLevel;
        _backgroundLevel = properties.BackgroundLevel;
        _keyEdge = properties.KeyEdge;
        _spillSuppression = properties.SpillSuppression;
        _flareSuppression = properties.FlareSuppression;
        _brightness = properties.Brightness;
        _contrast = properties.Contrast;
        _saturation = properties.Saturation;
        _red = properties.Red;
        _green = properties.Green;
        _blue = properties.Blue;
    }

    private void SerializeInternal(byte[] buffer)
    {
        buffer.WriteUInt16BigEndian((ushort)Flag, 0);
    }
}
