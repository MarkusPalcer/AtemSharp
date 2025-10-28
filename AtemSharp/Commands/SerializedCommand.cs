using AtemSharp.Enums;

namespace AtemSharp.Commands;

/// <summary>
/// Base command interface for serializable commands
/// </summary>
public abstract class SerializedCommand : ICommand
{
	/// <summary>
	/// Property change flags
	/// </summary>
	public uint Flag { get; internal set; }

    /// <summary>
    /// Serialize command to binary stream
    /// </summary>
    /// <param name="version">Protocol version</param>
    /// <returns>Serialized command data</returns>
    public abstract byte[] Serialize(ProtocolVersion version);
}
