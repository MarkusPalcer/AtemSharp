using AtemSharp.Enums;
using AtemSharp.Lib;
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
    public int MixEffectIndex { get; init; }

    /// <summary>
    /// Upstream keyer index (0-based)
    /// </summary>
    public int KeyerIndex { get; init; }

    /// <summary>
    /// Whether key frame A is set
    /// </summary>
    public bool IsASet { get; init; }

    /// <summary>
    /// Whether key frame B is set
    /// </summary>
    public bool IsBSet { get; init; }

    /// <summary>
    /// Running to key frame flags
    /// </summary>
    public IsAtKeyFrame IsAtKeyFrame { get; init; }

    /// <summary>
    /// Running to infinite index
    /// </summary>
    public int RunToInfiniteIndex { get; init; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    public static MixEffectKeyFlyPropertiesGetCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        var mixEffectIndex = rawCommand.ReadUInt8(0);
        var keyerIndex = rawCommand.ReadUInt8(1);
        var isASet = rawCommand.ReadBoolean(2);
        var isBSet = rawCommand.ReadBoolean(3);
        var isAtKeyFrame = (IsAtKeyFrame)rawCommand.ReadUInt8(4);
        var runToInfiniteIndex = rawCommand.ReadUInt8(5);

        return new MixEffectKeyFlyPropertiesGetCommand
        {
            MixEffectIndex = mixEffectIndex,
            KeyerIndex = keyerIndex,
            IsASet = isASet,
            IsBSet = isBSet,
            IsAtKeyFrame = isAtKeyFrame,
            RunToInfiniteIndex = runToInfiniteIndex
        };
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
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
        keyer.FlyProperties.IsAtKeyFrame = IsAtKeyFrame;
        keyer.FlyProperties.RunToInfiniteIndex = RunToInfiniteIndex;
    }
}
