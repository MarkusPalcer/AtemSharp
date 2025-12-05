using System.Diagnostics.CodeAnalysis;
using AtemSharp.State.Info;

namespace AtemSharp.Attributes;

[AttributeUsage(AttributeTargets.Class)]
[ExcludeFromCodeCoverage]
public class CommandAttribute : Attribute
{
	/// <summary>
	/// Raw command name for ATEM protocol
	/// </summary>
	public string RawName { get; }

	/// <summary>
	/// Minimum protocol version required for this command
	/// </summary>
	public ProtocolVersion? MinimumVersion { get; }

	/// <summary>
	/// Create a command attribute with just the raw name
	/// </summary>
	/// <param name="rawName">Raw command name for ATEM protocol</param>
	public CommandAttribute(string rawName)
	{
		RawName = rawName;
		MinimumVersion = null;
	}

	/// <summary>
	/// Create a command attribute with raw name and minimum version
	/// </summary>
	/// <param name="rawName">Raw command name for ATEM protocol</param>
	/// <param name="minimumVersion">Minimum protocol version required</param>
	public CommandAttribute(string rawName, ProtocolVersion minimumVersion)
	{
		RawName = rawName;
		MinimumVersion = minimumVersion;
	}
}
