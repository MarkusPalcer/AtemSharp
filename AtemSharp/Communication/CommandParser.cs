using System.Reflection;
using AtemSharp.Commands;
using AtemSharp.Commands.DeviceProfile;
using AtemSharp.State.Info;

namespace AtemSharp.Communication;

/// <summary>
/// Parses ATEM commands from raw packet data using reflection-based command registry
/// </summary>
public class CommandParser
{
	private static readonly Dictionary<string, List<Type>> CommandRegistry = new();
	private static readonly Lock LockObject = new();
	private static bool _initialized;

	/// <summary>
	/// Current protocol version for parsing commands
	/// </summary>
	public ProtocolVersion Version { get; set; } = ProtocolVersion.V7_2;

	static CommandParser()
	{
		InitializeCommandRegistry();
	}

	/// <summary>
	/// Initialize the command registry using reflection to find all command types
	/// </summary>
	private static void InitializeCommandRegistry()
	{
		if (_initialized) return;
		using var scope = LockObject.EnterScope();

		if (_initialized) return;

		// Scan the main assembly and loaded assemblies for command types
		var assemblies = new List<Assembly> { Assembly.GetExecutingAssembly() };

		// Also scan any loaded assemblies that might contain test commands
		assemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies()
			.Where(a => a.FullName?.Contains("AtemSharp") == true));

		var commandTypes = assemblies
			.SelectMany(assembly => assembly.GetTypes())
			.Where(t => t is { IsClass: true, IsAbstract: false } &&
			            typeof(IDeserializedCommand).IsAssignableFrom(t) &&
			            t.GetCustomAttribute<CommandAttribute>() != null);

		foreach (var type in commandTypes)
		{
			var attr = type.GetCustomAttribute<CommandAttribute>()!;

			// Store all versions of commands (matches TypeScript Array<CommandConstructor> approach)
			if (!CommandRegistry.ContainsKey(attr.RawName))
			{
				CommandRegistry[attr.RawName] = [];
			}
			CommandRegistry[attr.RawName].Add(type);
		}

		_initialized = true;
	}

	/// <summary>
	/// Get the appropriate command type for a given raw name and current protocol version
	/// (matches TypeScript commandFromRawName logic in atemCommandParser.ts:24-50)
	/// </summary>
	/// <param name="rawName">4-character command name</param>
	/// <returns>Best matching command type or null if not found/supported</returns>
	private Type? GetCommandTypeForVersion(string rawName)
	{
		if (!CommandRegistry.TryGetValue(rawName, out var commandTypes))
		{
			return null;
		}

		// Edge case for the version command itself (matches TypeScript line 29-31)
		if (Version == ProtocolVersion.V7_2) // Default/initial version
		{
			return commandTypes[0];
		}

		// Find baseline command (no minimum version requirement)
		var baseline = commandTypes.FirstOrDefault(type =>
		{
			var attr = type.GetCustomAttribute<CommandAttribute>()!;
			return !attr.MinimumVersion.HasValue;
		});

		// Find all overrides that are compatible with current version
		var overrides = commandTypes.Where(type =>
		{
			var attr = type.GetCustomAttribute<CommandAttribute>()!;
			return attr.MinimumVersion.HasValue && attr.MinimumVersion <= Version;
		}).ToList();

		// If no overrides, return baseline (matches TypeScript line 36)
		if (overrides.Count == 0) return baseline;

		// Find the highest version override (matches TypeScript lines 38-48)
		var highestProtoCommand = overrides[0];
		foreach (var candidate in overrides)
		{
			var highestAttr = highestProtoCommand.GetCustomAttribute<CommandAttribute>()!;
			var candidateAttr = candidate.GetCustomAttribute<CommandAttribute>()!;

			if (highestAttr.MinimumVersion.HasValue &&
			    candidateAttr.MinimumVersion.HasValue &&
			    candidateAttr.MinimumVersion > highestAttr.MinimumVersion)
			{
				highestProtoCommand = candidate;
			}
		}

		return highestProtoCommand;
	}

    internal delegate IDeserializedCommand DeserializeCommand(ReadOnlySpan<byte> data, ProtocolVersion version);

	/// <summary>
	/// Parse a command from raw name and binary data
	/// </summary>
	/// <param name="rawName">4-character command name from packet header</param>
	/// <param name="data">Command data stream</param>
	/// <returns>Parsed command instance or null if command is unknown/unsupported</returns>
	public IDeserializedCommand? ParseCommand(string rawName, ReadOnlySpan<byte> data)
	{
		var commandType = GetCommandTypeForVersion(rawName);
		if (commandType == null)
		{
			// Track unrecognized command (matches TypeScript emit('debug', `Unknown command ${name}`) behavior)
			AtemSwitcher.UnknownCommands.Add(rawName);
			return null;
		}

		try
		{
			// Call static Deserialize method (matches TypeScript cmdConstructor.deserialize pattern)
			var deserializeMethod = commandType.GetMethod("Deserialize",
			                                              BindingFlags.Static | BindingFlags.Public);

			if (deserializeMethod == null)
				throw new InvalidOperationException($"Command {commandType.Name} missing static Deserialize method");

            var command = deserializeMethod.CreateDelegate<DeserializeCommand>()(data, Version);

			// Update parser version if this is a VersionCommand (matches TypeScript behavior)
			if (command is VersionCommand versionCmd)
			{
				Version = versionCmd.Version;
			}

			return command;
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException($"Failed to deserialize command {rawName}: {ex.Message}", ex);
		}
	}

	/// <summary>
	/// Get a command type by its raw name (for testing/debugging)
	/// </summary>
	/// <param name="rawName">Raw command name</param>
	/// <returns>Command type if found, null otherwise</returns>
	public Type? GetCommandType(string rawName)
	{
		return GetCommandTypeForVersion(rawName);
	}

	/// <summary>
	/// Get all versions of a command type by its raw name (for testing/debugging)
	/// </summary>
	/// <param name="rawName">Raw command name</param>
	/// <returns>All registered command type versions</returns>
	public IReadOnlyList<Type> GetAllCommandVersions(string rawName)
	{
		return CommandRegistry.TryGetValue(rawName, out var commandTypes)
			? commandTypes.AsReadOnly()
			: new List<Type>().AsReadOnly();
	}

	/// <summary>
	/// Get all registered command names (for testing/debugging)
	/// </summary>
	/// <returns>Collection of all registered raw command names</returns>
	public IReadOnlyCollection<string> GetRegisteredCommands()
	{
		return CommandRegistry.Keys;
	}

	/// <summary>
	/// Force reinitialize the command registry (for testing purposes)
	/// </summary>
	internal static void ReinitializeForTesting()
	{
		using var scope = LockObject.EnterScope();
		_initialized = false;
		CommandRegistry.Clear();
		InitializeCommandRegistry();
	}
}
