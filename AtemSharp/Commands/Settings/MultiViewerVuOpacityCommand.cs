using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.Settings;

/// <summary>
/// Command to set MultiViewer VU opacity level
/// </summary>
[Command("VuMo")]
public class MultiViewerVuOpacityCommand : SerializedCommand, IDeserializedCommand
{
    private int _opacity;

    /// <summary>
    /// MultiViewer ID for this command
    /// </summary>
    public int MultiViewerId { get; set; }

    /// <summary>
    /// Create command initialized with current state values
    /// </summary>
    /// <param name="multiViewerId">MultiViewer ID (0-based)</param>
    /// <param name="currentState">Current ATEM state</param>
    /// <exception cref="InvalidIdError">Thrown if MultiViewer ID is invalid</exception>
    public MultiViewerVuOpacityCommand(int multiViewerId, AtemState currentState)
    {
        MultiViewerId = multiViewerId;

        // Validate MultiViewer exists
        if (currentState.Info.MultiViewer.Count == 0 || multiViewerId >= currentState.Info.MultiViewer.Count)
        {
            throw new InvalidIdError("MultiViewer", multiViewerId);
        }

        // Initialize from current state or defaults if no existing state
        var multiViewer = AtemStateUtil.GetMultiViewer(currentState, multiViewerId);
        
        // Initialize from current state (direct field access = no flags set)
        _opacity = multiViewer.VuOpacity;
    }

    /// <summary>
    /// Convenience constructor for setting opacity directly
    /// </summary>
    /// <param name="multiViewerId">MultiViewer ID (0-based)</param>
    /// <param name="opacity">VU opacity level (0-100)</param>
    /// <param name="currentState">Current ATEM state</param>
    public MultiViewerVuOpacityCommand(int multiViewerId, int opacity, AtemState currentState)
        : this(multiViewerId, currentState)
    {
        Opacity = opacity;
    }

    /// <summary>
    /// Parameterless constructor for deserialization
    /// </summary>
    public MultiViewerVuOpacityCommand()
    {
    }

    /// <summary>
    /// VU opacity level (0-100)
    /// </summary>
    public int Opacity
    {
        get => _opacity;
        set
        {
            if (value < 0 || value > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Opacity must be between 0 and 100");
            }
            
            _opacity = value;
            Flag |= 1 << 0;  // Automatic flag setting for opacity
        }
    }

    /// <inheritdoc />
    public override byte[] Serialize(ProtocolVersion version)
    {
        using var memoryStream = new MemoryStream(4);
        using var writer = new BinaryWriter(memoryStream);
        
        // Write MultiViewer ID as single byte
        writer.Write((byte)MultiViewerId);
        
        // Write opacity as single byte
        writer.Write((byte)Opacity);
        
        // Pad to 4 bytes total
        writer.Pad(2);
        
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <param name="protocolVersion">Protocol version used for deserialization</param>
    /// <returns>Deserialized command instance</returns>
    public static MultiViewerVuOpacityCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, System.Text.Encoding.Default, leaveOpen: true);

        var multiViewerId = reader.ReadByte();
        var opacity = reader.ReadByte();

        return new MultiViewerVuOpacityCommand
        {
            MultiViewerId = multiViewerId,
            Opacity = opacity
        };
    }

    /// <inheritdoc />
    public string[] ApplyToState(AtemState state)
    {
        // Validate state prerequisites (same pattern as TypeScript update commands)
        if (state.Info.MultiViewer.Count == 0 || MultiViewerId >= state.Info.MultiViewer.Count)
        {
            throw new InvalidIdError("MultiViewer", MultiViewerId);
        }

        // Get or create the MultiViewer and update its VU opacity
        var multiViewer = AtemStateUtil.GetMultiViewer(state, MultiViewerId);
        multiViewer.VuOpacity = Opacity;

        // Return the state path that was modified for change tracking
        return [$"settings.multiViewers.{MultiViewerId}.vuOpacity"];
    }
}