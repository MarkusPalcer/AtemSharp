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
	Task ConnectAsync(string address, int port = AtemConstants.DEFAULT_PORT, CancellationToken cancellationToken = default);

	/// <summary>
	/// Disconnects from the ATEM device
	/// </summary>
	Task DisconnectAsync();

    /// <summary>
    /// Sends a command to the ATEM device
    /// </summary>
    Task SendCommandAsync(SerializedCommand command);

    /// <summary>
    /// Sends a series of commands to the ATEM device
    /// </summary>
    Task SendCommandsAsync(IEnumerable<SerializedCommand> commands);
}
