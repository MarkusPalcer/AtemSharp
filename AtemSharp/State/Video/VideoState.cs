using System.Diagnostics.CodeAnalysis;
using AtemSharp.Types;

namespace AtemSharp.State.Video;

/// <summary>
/// Video state container for ATEM devices
/// </summary>
public class VideoState
{
    public VideoState()
    {
        Inputs = new ItemCollection<ushort, InputChannel.InputChannel>(id => new InputChannel.InputChannel { InputId = id });
        DownstreamKeyers = new ItemCollection<byte, DownstreamKeyer.DownstreamKeyer>(id => new DownstreamKeyer.DownstreamKeyer { Id = id });
        MixEffects = new ItemCollection<byte, MixEffect.MixEffect>(id => new MixEffect.MixEffect { Id = id });
        SuperSources = new ItemCollection<byte, SuperSource.SuperSource>(id => new SuperSource.SuperSource { Id = id });
        Auxiliaries = new ItemCollection<byte, AuxiliaryOutput>(id => new AuxiliaryOutput { Id = id });
    }

    /// <summary>
    /// Input channels available on this device
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ItemCollection<ushort, InputChannel.InputChannel> Inputs { get; }

    /// <summary>
    /// Mix effects available on this device
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ItemCollection<byte, MixEffect.MixEffect> MixEffects { get; }

    /// <summary>
    /// Downstream keyers available on this device
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ItemCollection<byte, DownstreamKeyer.DownstreamKeyer> DownstreamKeyers { get; }

    /// <summary>
    /// Auxiliary output sources
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ItemCollection<byte, AuxiliaryOutput> Auxiliaries { get; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ItemCollection<byte, SuperSource.SuperSource> SuperSources { get; }
}
