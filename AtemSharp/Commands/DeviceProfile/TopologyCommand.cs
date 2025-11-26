using AtemSharp.State;
using AtemSharp.State.Info;
using AtemSharp.State.Media;
using AtemSharp.State.Video;
using AtemSharp.State.Video.DownstreamKeyer;
using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Commands.DeviceProfile;

/// <summary>
/// Device topology command received from ATEM containing device capabilities and configuration
/// </summary>
[Command("_top")]
public class TopologyCommand : IDeserializedCommand
{
    /// <summary>
    /// Number of Mix Effect blocks available
    /// </summary>
    public int MixEffects { get; init; }

    /// <summary>
    /// Number of video sources available
    /// </summary>
    public int Sources { get; init; }

    /// <summary>
    /// Number of auxiliary outputs available
    /// </summary>
    public int Auxiliaries { get; init; }

    /// <summary>
    /// Number of mix minus outputs available
    /// </summary>
    public int MixMinusOutputs { get; init; }

    /// <summary>
    /// Number of media players available
    /// </summary>
    public int MediaPlayers { get; init; }

    /// <summary>
    /// Number of multiviewers available (-1 for older protocol versions)
    /// </summary>
    public int Multiviewers { get; init; } = -1;

    /// <summary>
    /// Number of serial ports available
    /// </summary>
    public int SerialPorts { get; init; }

    /// <summary>
    /// Maximum number of HyperDecks supported
    /// </summary>
    public int MaxHyperdecks { get; init; }

    /// <summary>
    /// Number of Digital Video Effects available
    /// </summary>
    // ReSharper disable once InconsistentNaming Domain Specific Acronym
    public int DigitalVideoEffects { get; init; }

    /// <summary>
    /// Number of stinger transitions available
    /// </summary>
    public int Stingers { get; init; }

    /// <summary>
    /// Number of SuperSource inputs available
    /// </summary>
    public int SuperSources { get; init; }

    /// <summary>
    /// Number of talkback channels available
    /// </summary>
    public int TalkbackChannels { get; init; }

    /// <summary>
    /// Number of downstream keyers available
    /// </summary>
    public int DownstreamKeyers { get; init; }

    /// <summary>
    /// Camera control capability
    /// </summary>
    public bool CameraControl { get; init; }

    /// <summary>
    /// Advanced chroma keyers available
    /// </summary>
    public bool AdvancedChromaKeyers { get; set; }

    /// <summary>
    /// Only configurable outputs available
    /// </summary>
    public bool OnlyConfigurableOutputs { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    public static TopologyCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        // Protocol version offset calculation (matching TypeScript logic)
        var v230Offset = protocolVersion > ProtocolVersion.V8_0_1 ? 1 : 0;

        var command = new TopologyCommand
        {
            MixEffects = rawCommand.ReadUInt8(0),
            Sources = rawCommand.ReadUInt8(1),
            DownstreamKeyers = rawCommand.ReadUInt8(2),
            Auxiliaries = rawCommand.ReadUInt8(3),
            MixMinusOutputs = rawCommand.ReadUInt8(4),
            MediaPlayers = rawCommand.ReadUInt8(5),
            Multiviewers = v230Offset > 0 ? rawCommand.ReadUInt8(6) : -1,
            SerialPorts = rawCommand.ReadUInt8(6 + v230Offset),
            MaxHyperdecks = rawCommand.ReadUInt8(7 + v230Offset),
            DigitalVideoEffects = rawCommand.ReadUInt8(8 + v230Offset),
            Stingers = rawCommand.ReadUInt8(9 + v230Offset),
            SuperSources = rawCommand.ReadUInt8(10 + v230Offset),
            TalkbackChannels = rawCommand.ReadUInt8(12 + v230Offset),
            CameraControl = rawCommand.ReadBoolean(17 + v230Offset),
            AdvancedChromaKeyers = false,
            OnlyConfigurableOutputs = false
        };

        if (rawCommand.Length > 20)
        {
            command.AdvancedChromaKeyers = rawCommand.ReadBoolean(21 + v230Offset);
            command.OnlyConfigurableOutputs = rawCommand.ReadBoolean(22 + v230Offset);
        }

        return command;
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Create capabilities object with all the topology data
        state.Info.Capabilities.MixEffects = MixEffects;
        state.Info.Capabilities.Sources = Sources;
        state.Info.Capabilities.Auxiliaries = Auxiliaries;
        state.Info.Capabilities.MixMinusOutputs = MixMinusOutputs;
        state.Info.Capabilities.MediaPlayers = MediaPlayers;
        state.Info.Capabilities.MultiViewers = Multiviewers;
        state.Info.Capabilities.SerialPorts = SerialPorts;
        state.Info.Capabilities.MaxHyperdecks = MaxHyperdecks;
        state.Info.Capabilities.DigitalVideoEffects = DigitalVideoEffects;
        state.Info.Capabilities.Stingers = Stingers;
        state.Info.Capabilities.SuperSources = SuperSources;
        state.Info.Capabilities.TalkbackChannels = TalkbackChannels;
        state.Info.Capabilities.DownstreamKeyers = DownstreamKeyers;
        state.Info.Capabilities.CameraControl = CameraControl;
        state.Info.Capabilities.AdvancedChromaKeyers = AdvancedChromaKeyers;
        state.Info.Capabilities.OnlyConfigurableOutputs = OnlyConfigurableOutputs;

        // Create arrays now that their sizes are known
        state.Info.MixEffects = AtemStateUtil.CreateArray<MixEffectInfo>(MixEffects);
        state.Video.MixEffects = AtemStateUtil.CreateArray<MixEffect>(MixEffects);

        state.Video.Auxiliaries = AtemStateUtil.CreateArray<AuxiliaryOutput>(Auxiliaries);

        state.Media.Players = AtemStateUtil.CreateArray<MediaPlayer>(MediaPlayers);

        state.Info.SuperSources = new SuperSourceInfo[SuperSources];
        state.Video.SuperSources = AtemStateUtil.CreateArray<State.Video.SuperSource.SuperSource>(SuperSources);

        state.Video.DownstreamKeyers = AtemStateUtil.CreateArray<DownstreamKeyer>(DownstreamKeyers);

        state.Info.MultiViewer.Count = Multiviewers;
        state.Info.MultiViewer.WindowCount = Multiviewers > 0 ? 10 : 0;
    }
}
