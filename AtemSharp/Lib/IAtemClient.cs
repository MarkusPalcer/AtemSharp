using System.Threading.Tasks.Dataflow;
using AtemSharp.Commands;
using AtemSharp.Constants;

namespace AtemSharp.Lib;

public interface IAtemClient : IAsyncDisposable
{
    public IReceivableSourceBlock<IDeserializedCommand> ReceivedCommands { get; }

	/// <summary>
	/// Connects to the specified ATEM device
	/// </summary>
	/// <param name="address">IP address of the ATEM device</param>
	/// <param name="port">Port number (default: 9910)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Task that completes when connected</returns>
	Task ConnectAsync(string address, int port = AtemConstants.DEFAULT_PORT, CancellationToken cancellationToken = default);

	/// <summary>
	/// Disconnects from the ATEM device
	/// </summary>
	/// <returns>Task that completes when disconnected</returns>
	Task DisconnectAsync();

    Task SendCommand(SerializedCommand command);
}
