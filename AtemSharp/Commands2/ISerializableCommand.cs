using System.Runtime.InteropServices.ComTypes;
using AtemSharp.Enums;

namespace AtemSharp.Commands2;

public interface ISerializableCommand
{
	/// <summary>
	/// Serialize command to binary data
	/// </summary>
	Stream Serialize(ProtocolVersion version);
}