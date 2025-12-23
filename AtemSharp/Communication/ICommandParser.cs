using AtemSharp.Commands;
using AtemSharp.State.Info;

namespace AtemSharp.Communication;

internal interface ICommandParser
{
    /// <summary>
    /// Current protocol version for parsing commands
    /// </summary>
    ProtocolVersion Version { get; set; }

    /// <summary>
    /// Parse a command from raw name and binary data
    /// </summary>
    /// <param name="rawName">4-character command name from packet header</param>
    /// <param name="data">Command data stream</param>
    /// <returns>Parsed command instance or null if command is unknown/unsupported</returns>
    IDeserializedCommand? ParseCommand(string rawName, ReadOnlySpan<byte> data);
}
