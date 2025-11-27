using System.Threading.Tasks.Dataflow;
using AtemSharp.Commands;
using AtemSharp.Constants;

namespace AtemSharp.Communication;

internal interface IAtemClient : IAsyncDisposable
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

    /// <summary>
    /// Sends a command to the ATEM device
    /// </summary>
    Task SendCommandAsync(SerializedCommand command);

    /// <summary>
    /// Sends a series of commands to the ATEM device
    /// </summary>
    Task SendCommandsAsync(IEnumerable<SerializedCommand> commands);
}
