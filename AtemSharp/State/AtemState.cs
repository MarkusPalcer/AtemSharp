using AtemSharp.Enums;

namespace AtemSharp.State;

/// <summary>
/// Device information state
/// </summary>
public class DeviceInfo
{
    public ProtocolVersion? ApiVersion { get; set; }
    public Model? Model { get; set; }
    public string? ProductName { get; set; }
    public DeviceCapabilities? Capabilities { get; set; }
}

/// <summary>
/// Device capabilities
/// </summary>
public class DeviceCapabilities
{
    public int MixEffects { get; set; }
    public int VideoSources { get; set; }
    public int AudioSources { get; set; }
    public int DownstreamKeyers { get; set; }
    public int UpstreamKeyers { get; set; }
    public int Auxiliaries { get; set; }
    public int ColorGenerators { get; set; }
    public int MultiviewerOutputs { get; set; }
    public int MediaPlayers { get; set; }
    public int SerialPorts { get; set; }
    public int TalkbackChannels { get; set; }
    public int DVE { get; set; }
    public int Stingers { get; set; }
    public int SuperSources { get; set; }
    public bool HasCameraControl { get; set; }
    public bool HasAdvancedChromaKeyer { get; set; }
    public bool HasOnlyConfigurableOutputs { get; set; }
}

/// <summary>
/// Mix Effect state
/// </summary>
public class MixEffectState
{
    public int? ProgramInput { get; set; }
    public int? PreviewInput { get; set; }
    public bool InTransition { get; set; }
    public TransitionStyle TransitionStyle { get; set; }
    public TransitionSelection TransitionSelection { get; set; }
    
    // TODO: Add more MixEffect properties as needed
}

/// <summary>
/// Video state container
/// </summary>
public class VideoState
{
    public Dictionary<int, MixEffectState> MixEffects { get; set; } = new();
    
    // TODO: Add more video state properties
}

/// <summary>
/// Settings state
/// </summary>
public class SettingsState
{
    public VideoMode? VideoMode { get; set; }
    
    // TODO: Add more settings
}

/// <summary>
/// Input channel state
/// </summary>
public class InputChannelState
{
    public int InputId { get; set; }
    public string? Name { get; set; }
    public string? ShortName { get; set; }
    public bool IsExternal { get; set; }
    public ExternalPortType? PortType { get; set; }
    public SourceAvailability Availability { get; set; }
    
    // TODO: Add more input properties
}

/// <summary>
/// Main ATEM state container
/// </summary>
public class AtemState
{
    public DeviceInfo Info { get; set; } = new();
    public VideoState Video { get; set; } = new();
    public SettingsState Settings { get; set; } = new();
    public Dictionary<int, InputChannelState> Inputs { get; set; } = new();
    public AudioState? Audio { get; set; }
    
    // TODO: Add other state containers as needed
    // public MediaState Media { get; set; }
    // public MacroState Macro { get; set; }
    // public RecordingState? Recording { get; set; }
    // public StreamingState? Streaming { get; set; }
}

/// <summary>
/// Exception thrown when an invalid ID is used
/// </summary>
public class InvalidIdError : Exception
{
    public InvalidIdError(string entityType, int id) 
        : base($"Invalid {entityType} id: {id}")
    {
    }
    
    public InvalidIdError(string entityType, string id) 
        : base($"Invalid {entityType} id: {id}")
    {
    }
}

/// <summary>
/// Utility methods for ATEM state management
/// </summary>
public static class AtemStateUtil
{
    /// <summary>
    /// Create a new ATEM state with default values
    /// </summary>
    /// <returns>New ATEM state instance</returns>
    public static AtemState Create()
    {
        return new AtemState();
    }
    
    /// <summary>
    /// Get or create a mix effect state
    /// </summary>
    /// <param name="state">ATEM state</param>
    /// <param name="mixEffect">Mix effect index</param>
    /// <returns>Mix effect state</returns>
    public static MixEffectState GetMixEffect(AtemState state, int mixEffect)
    {
        if (!state.Video.MixEffects.TryGetValue(mixEffect, out var me))
        {
            me = new MixEffectState();
            state.Video.MixEffects[mixEffect] = me;
        }
        return me;
    }
}