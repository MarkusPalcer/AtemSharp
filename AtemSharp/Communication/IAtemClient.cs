using System.Threading.Tasks.Dataflow;
using AtemSharp.Commands;
using AtemSharp.Constants;

namespace AtemSharp.Communication;

public interface IAtemClient : IAsyncDisposable, ICommandSender
{
    IReceivableSourceBlock<IDeserializedCommand> ReceivedCommands { get; }

	/// <summary>
	/// Connects to the specified ATEM device
	/// </summary>
	/// <param name="address">IP address of the ATEM device</param>
	/// <param name="port">Port number (default: 9910)</param>
	Task ConnectAsync(string address, int port = AtemConstants.DefaultPort);

	/// <summary>
	/// Disconnects from the ATEM device
	/// </summary>
	Task DisconnectAsync();
}
