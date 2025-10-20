using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

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
    public int DVEs { get; init; }

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
            DVEs = rawCommand.ReadUInt8(8 + v230Offset),
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

    /// <summary>
    /// Apply the command's values to the ATEM state
    /// </summary>
    /// <param name="state">Current ATEM state to update</param>
    /// <returns>Paths indicating what was changed in the state</returns>
    public void ApplyToState(AtemState state)
    {
        // Create capabilities object with all the topology data
        state.Info.Capabilities = new AtemCapabilities
        {
            MixEffects = MixEffects,
            Sources = Sources,
            Auxiliaries = Auxiliaries,
            MixMinusOutputs = MixMinusOutputs,
            MediaPlayers = MediaPlayers,
            MultiViewers = Multiviewers,
            SerialPorts = SerialPorts,
            MaxHyperdecks = MaxHyperdecks,
            DVEs = DVEs,
            Stingers = Stingers,
            SuperSources = SuperSources,
            TalkbackChannels = TalkbackChannels,
            DownstreamKeyers = DownstreamKeyers,
            CameraControl = CameraControl,
            AdvancedChromaKeyers = AdvancedChromaKeyers,
            OnlyConfigurableOutputs = OnlyConfigurableOutputs
        };

        state.Video.DownstreamKeyers = AtemStateUtil.CreateArray<DownstreamKeyer>(DownstreamKeyers);
        state.Video.DownstreamKeyers.ForEachWithIndex((dsk, index) => dsk.Id = (byte)index);

        state.Media.Players = AtemStateUtil.CreateArray<MediaPlayer>(MediaPlayers);
        state.Media.Players.ForEachWithIndex((player, index) => player.Id = (byte)index);

        state.Info.MultiViewer.Count = Multiviewers;
        if (Multiviewers > 0)
        {
            state.Info.MultiViewer.WindowCount = 10;
        } else if (Multiviewers < 0)
        {
            state.Info.MultiViewer.WindowCount = -1;
        }
        else
        {
            state.Info.MultiViewer.WindowCount = 0;
        }
    }
}
