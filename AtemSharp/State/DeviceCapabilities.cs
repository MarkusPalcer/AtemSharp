namespace AtemSharp.State;

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