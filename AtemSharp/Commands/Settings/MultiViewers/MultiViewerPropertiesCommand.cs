using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Settings.MultiViewers;

/// <summary>
/// Command to set MultiViewer properties (layout and program/preview swap)
/// </summary>
[Command("CMvP", ProtocolVersion.V8_0)]
public class MultiViewerPropertiesCommand : SerializedCommand
{
    private MultiViewerLayout _layout;
    private bool _programPreviewSwapped;

    /// <summary>
    /// MultiViewer ID for this command
    /// </summary>
    public int MultiViewerId { get; }

    /// <summary>
    /// Create command initialized with current state values
    /// </summary>
    /// <param name="multiViewerId">MultiViewer ID (0-based)</param>
    /// <param name="currentState">Current ATEM state</param>
    /// <exception cref="InvalidIdError">Thrown if MultiViewer ID is invalid</exception>
    public MultiViewerPropertiesCommand(int multiViewerId, AtemState currentState)
    {
        MultiViewerId = multiViewerId;

        // Validate MultiViewer exists
        if (currentState.Info.MultiViewer.Count == 0 || multiViewerId >= currentState.Info.MultiViewer.Count)
        {
            throw new InvalidIdError("MultiViewer", multiViewerId);
        }

        // Initialize from current state or defaults if no existing state
        var multiViewer = AtemStateUtil.GetMultiViewer(currentState, multiViewerId);
        if (multiViewer.Properties != null)
        {
            // Initialize from current state (direct field access = no flags set)
            _layout = multiViewer.Properties.Layout;
            _programPreviewSwapped = multiViewer.Properties.ProgramPreviewSwapped;
        }
        else
        {
            // Initialize with defaults (property access = flags set)
            Layout = MultiViewerLayout.Default;
            ProgramPreviewSwapped = false;
        }
    }

    /// <summary>
    /// MultiViewer layout configuration
    /// </summary>
    public MultiViewerLayout Layout
    {
        get => _layout;
        set
        {
            _layout = value;
            Flag |= 1 << 0;  // Automatic flag setting for layout
        }
    }

    /// <summary>
    /// Whether program and preview outputs are swapped
    /// </summary>
    public bool ProgramPreviewSwapped
    {
        get => _programPreviewSwapped;
        set
        {
            _programPreviewSwapped = value;
            Flag |= 1 << 1;  // Automatic flag setting for programPreviewSwapped
        }
    }

    /// <inheritdoc />
    public override byte[] Serialize(ProtocolVersion version)
    {
        using var memoryStream = new MemoryStream(4);
        using var writer = new BinaryWriter(memoryStream);

        // Write flag as single byte (matching TypeScript pattern)
        writer.Write((byte)Flag);

        // Write MultiViewer ID
        writer.Write((byte)MultiViewerId);

        // Write layout
        writer.Write((byte)Layout);

        // Write program/preview swapped flag
        writer.WriteBoolean(ProgramPreviewSwapped);

        return memoryStream.ToArray();
    }
}
