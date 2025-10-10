namespace AtemSharp.State;

/// <summary>
/// ATEM device capabilities and hardware configuration
/// </summary>
public class AtemCapabilities
{
    /// <summary>
    /// Number of Mix Effect blocks available
    /// </summary>
    public int MixEffects { get; set; }

    /// <summary>
    /// Number of video sources available
    /// </summary>
    public int Sources { get; set; }

    /// <summary>
    /// Number of auxiliary outputs available
    /// </summary>
    public int Auxiliaries { get; set; }

    /// <summary>
    /// Number of mix minus outputs available
    /// </summary>
    public int MixMinusOutputs { get; set; }

    /// <summary>
    /// Number of media players available
    /// </summary>
    public int MediaPlayers { get; set; }

    /// <summary>
    /// Number of serial ports available
    /// </summary>
    public int SerialPorts { get; set; }

    /// <summary>
    /// Maximum number of HyperDecks supported
    /// </summary>
    public int MaxHyperdecks { get; set; }

    /// <summary>
    /// Number of Digital Video Effects available
    /// </summary>
    // ReSharper disable once InconsistentNaming - Domain Specific Acronym
    public int DVEs { get; set; }

    /// <summary>
    /// Number of stinger transitions available
    /// </summary>
    public int Stingers { get; set; }

    /// <summary>
    /// Number of SuperSource inputs available
    /// </summary>
    public int SuperSources { get; set; }

    /// <summary>
    /// Number of talkback channels available
    /// </summary>
    public int TalkbackChannels { get; set; }

    /// <summary>
    /// Number of downstream keyers available
    /// </summary>
    public int DownstreamKeyers { get; set; }

    /// <summary>
    /// Camera control capability
    /// </summary>
    public bool CameraControl { get; set; }

    /// <summary>
    /// Advanced chroma keyers available
    /// </summary>
    public bool AdvancedChromaKeyers { get; set; }

    /// <summary>
    /// Only configurable outputs available
    /// </summary>
    public bool OnlyConfigurableOutputs { get; set; }

    /// <summary>
    /// Number of multiviewers available
    /// </summary>
    public int MultiViewers { get; set; }
}