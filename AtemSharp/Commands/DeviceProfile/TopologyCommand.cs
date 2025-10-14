using System.Text;
using AtemSharp.Enums;
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
    /// Number of multiviewers available (-1 for older protocol versions)
    /// </summary>
    public int Multiviewers { get; set; } = -1;

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
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <param name="protocolVersion">Protocol version used for deserialization</param>
    /// <returns>Deserialized command instance</returns>
    public static TopologyCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);

        // Protocol version offset calculation (matching TypeScript logic)
        var v230offset = protocolVersion > ProtocolVersion.V8_0_1 ? 1 : 0;
        var isOlderProtocol = protocolVersion <= ProtocolVersion.V8_0;

        var command = new TopologyCommand
        {
            MixEffects = reader.ReadByte(),              // offset 0
            Sources = reader.ReadByte(),                 // offset 1
            DownstreamKeyers = reader.ReadByte(),        // offset 2
            Auxiliaries = reader.ReadByte(),             // offset 3
            MixMinusOutputs = reader.ReadByte(),         // offset 4
            MediaPlayers = reader.ReadByte(),            // offset 5
        };

        // Multiviewers field only available in newer protocol versions
        if (v230offset > 0)
        {
            command.Multiviewers = reader.ReadByte();    // offset 6 (newer versions only)
        }
        else
        {
            command.Multiviewers = -1;                   // Not available in older protocols
        }

        command.SerialPorts = reader.ReadByte();         // offset 6 + v230offset
        command.MaxHyperdecks = reader.ReadByte();       // offset 7 + v230offset
        command.DVEs = reader.ReadByte();                // offset 8 + v230offset
        command.Stingers = reader.ReadByte();            // offset 9 + v230offset
        command.SuperSources = reader.ReadByte();        // offset 10 + v230offset

        // Skip one byte (offset 11 + v230offset is unused)
        reader.ReadByte();

        // For older protocols, TalkbackChannels and advanced features are not available
        if (isOlderProtocol)
        {
            // Set features to 0/false for older protocols that don't support them
            command.TalkbackChannels = 0;
            command.CameraControl = false;
            command.AdvancedChromaKeyers = false;
            command.OnlyConfigurableOutputs = false;
        }
        else
        {
            command.TalkbackChannels = reader.ReadByte();    // offset 12 + v230offset

            // Skip bytes to reach camera control flag (offset 17 + v230offset)
            for (var i = 0; i < 4; i++)
            {
                reader.ReadByte();
            }

            command.CameraControl = reader.ReadBoolean();;  // offset 17 + v230offset

            // Advanced features are only available if buffer has enough data
            var totalLength = (int)stream.Length;
            if (totalLength > 20)
            {
                // Skip to advanced features position
                var currentPosition = (int)stream.Position;
                var advancedFeaturesStart = 21 + v230offset;
                var skipBytes = advancedFeaturesStart - currentPosition;

                // Skip bytes to reach advanced features if needed
                for (var i = 0; i < skipBytes && stream.Position < stream.Length; i++)
                {
                    reader.ReadByte();
                }

                // Read advanced features if available
                if (stream.Position < stream.Length)
                {
                    command.AdvancedChromaKeyers = reader.ReadBoolean();;

                    if (stream.Position < stream.Length)
                    {
                        command.OnlyConfigurableOutputs = reader.ReadBoolean();;
                    }
                }
            }
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

        // Create multiviewer info if multiviewers are available
        if (Multiviewers > 0)
        {
            state.Info.MultiViewer = new MultiViewerInfo
            {
                Count = Multiviewers,
                WindowCount = 10  // Default as per TypeScript implementation
            };
        }
    }
}
