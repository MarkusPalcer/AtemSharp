using AtemSharp.Enums;

namespace AtemSharp.State;

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