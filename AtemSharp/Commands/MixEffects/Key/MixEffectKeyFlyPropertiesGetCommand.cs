using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Key;

/// <summary>
/// Command received from ATEM device containing upstream keyer fly properties
/// </summary>
[Command("KeFS")]
public class MixEffectKeyFlyPropertiesGetCommand : IDeserializedCommand
{
    /// <summary>
    /// Mix effect index (0-based)
    /// </summary>
    public int MixEffectIndex { get; set; }

    /// <summary>
    /// Upstream keyer index (0-based)
    /// </summary>
    public int KeyerIndex { get; set; }

    /// <summary>
    /// Whether key frame A is set
    /// </summary>
    public bool IsASet { get; set; }

    /// <summary>
    /// Whether key frame B is set
    /// </summary>
    public bool IsBSet { get; set; }

    /// <summary>
    /// Running to key frame flags
    /// </summary>
    public IsAtKeyFrame RunningToKeyFrame { get; set; }

    /// <summary>
    /// Running to infinite index
    /// </summary>
    public int RunningToInfinite { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <param name="protocolVersion">Protocol version used for deserialization</param>
    /// <returns>Deserialized command instance</returns>
    public static MixEffectKeyFlyPropertiesGetCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, System.Text.Encoding.Default, leaveOpen: true);

        var mixEffectIndex = reader.ReadByte();           // byte 0
        var keyerIndex = reader.ReadByte();               // byte 1
        var isASet = reader.ReadBoolean();              // byte 2
        var isBSet = reader.ReadBoolean();              // byte 3
        var runningToKeyFrame = (IsAtKeyFrame)reader.ReadByte(); // byte 4
        var runningToInfinite = reader.ReadByte();       // byte 5
        reader.ReadByte();                                // byte 6 - padding
        reader.ReadByte();                                // byte 7 - padding

        return new MixEffectKeyFlyPropertiesGetCommand
        {
            MixEffectIndex = mixEffectIndex,
            KeyerIndex = keyerIndex,
            IsASet = isASet,
            IsBSet = isBSet,
            RunningToKeyFrame = runningToKeyFrame,
            RunningToInfinite = runningToInfinite
        };
    }

    /// <inheritdoc />
    public string[] ApplyToState(AtemState state)
    {
        // Validate mix effect index - need to get capabilities info
        if (state.Info.Capabilities == null || MixEffectIndex >= state.Info.Capabilities.MixEffects)
        {
            throw new InvalidIdError("MixEffect", MixEffectIndex);
        }

        // TODO: Add validation for keyer index when capabilities include upstream keyer count
        // For now, we'll proceed with state updates

        // Get or create the mix effect
        var mixEffect = state.Video.MixEffects.GetOrCreate(MixEffectIndex);
        
        // Get or create the upstream keyer
        var keyer = mixEffect.UpstreamKeyers.GetOrCreate(KeyerIndex);
        keyer.Index = KeyerIndex;
        
        // Update fly properties
        keyer.FlyProperties ??= new UpstreamKeyerFlyProperties();

        keyer.FlyProperties.IsASet = IsASet;
        keyer.FlyProperties.IsBSet = IsBSet;
        keyer.FlyProperties.IsAtKeyFrame = RunningToKeyFrame;
        keyer.FlyProperties.RunToInfiniteIndex = RunningToInfinite;

        // Return the state path that was modified
        return [$"video.mixEffects.{MixEffectIndex}.upstreamKeyers.{KeyerIndex}.flyProperties"];
    }
}