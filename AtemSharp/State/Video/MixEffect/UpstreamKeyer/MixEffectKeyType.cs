namespace AtemSharp.State.Video.MixEffect.UpstreamKeyer;

/// <summary>
/// Types of keying effects available for upstream keyers
/// </summary>
public enum MixEffectKeyType : byte
{
    /// <summary>
    /// Luma key type
    /// </summary>
    Luma = 0,

    /// <summary>
    /// Chroma key type
    /// </summary>
    Chroma = 1,

    /// <summary>
    /// Pattern key type
    /// </summary>
    Pattern = 2,

    /// <summary>
    /// DVE (Digital Video Effect) key type
    /// </summary>
    DVE = 3
}
