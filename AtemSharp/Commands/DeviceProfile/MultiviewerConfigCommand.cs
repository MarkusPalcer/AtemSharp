using System.Text;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.DeviceProfile;

[Command("_MvC")]
public class MultiviewerConfigCommand : IDeserializedCommand
{
	public int Count { get; set; }
	public int WindowCount { get; set; }

	public static MultiviewerConfigCommand Deserialize(Stream stream, ProtocolVersion version)
	{
		using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);

		var result = new MultiviewerConfigCommand();

		result.Count = version >= ProtocolVersion.V8_1_1 ? 0 : reader.ReadByte();
		result.WindowCount = reader.ReadByte();

		return result;
	}
	
	public string[] ApplyToState(AtemState state)
	{
		state.MultiViewer ??= new MultiViewerState();

		state.MultiViewer.Count = Count;
		state.MultiViewer.WindowCount = WindowCount;
		return ["multiViewer"];
	}
}