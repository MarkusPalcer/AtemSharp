using System.Runtime.InteropServices.ComTypes;
using AtemSharp.Enums;

namespace AtemSharp.Commands2;

public abstract class BasicWritableCommand : ISerializableCommand
{

	/// <summary>
	/// Raw command name
	/// </summary>
	public static string? RawName { get; protected set; }
    
	/// <summary>
	/// Minimum protocol version required
	/// </summary>
	public static ProtocolVersion? MinimumVersion { get; protected set; }
	
	
	/// <inheritdoc />
	public abstract Stream Serialize(ProtocolVersion version);
}