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