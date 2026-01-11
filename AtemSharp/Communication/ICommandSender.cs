using AtemSharp.Commands;

namespace AtemSharp.Communication;

public interface ICommandSender
{
    /// <summary>
    /// Sends a command to the ATEM device
    /// </summary>
    Task SendCommandAsync(SerializedCommand command);

    /// <summary>
    /// Sends a series of commands to the ATEM device
    /// </summary>
    Task SendCommandsAsync(IEnumerable<SerializedCommand> commands);
}
