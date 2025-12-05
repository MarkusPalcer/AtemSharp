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
    private readonly Dictionary<string, SortedList<ProtocolVersion,Type>> _commandRegistry = new();

    /// <summary>
    /// Current protocol version for parsing commands
    /// </summary>
    public ProtocolVersion Version { get; set; } = ProtocolVersion.V7_2;

    public CommandParser()
    {
        InitializeCommandRegistry();
    }

    internal void AddCommandsFromAssemblyOf<T>()
    {
        AddCommandsFromAssembly(typeof(T).Assembly);
    }

    private void AddCommandsFromAssembly(Assembly assembly)
    {
        var commandTypes = assembly.GetTypes().Where(t => typeof(IDeserializedCommand).IsAssignableFrom(t) &&
                                                          t.GetCustomAttribute<CommandAttribute>() != null);

        foreach (var type in commandTypes)
        {
            var attr = type.GetCustomAttribute<CommandAttribute>()!;

            // Store all versions of commands (matches TypeScript Array<CommandConstructor> approach)
            if (!_commandRegistry.ContainsKey(attr.RawName))
            {
                _commandRegistry[attr.RawName] = [];
            }

            _commandRegistry[attr.RawName].Add(attr.MinimumVersion ?? ProtocolVersion.Unknown, type);
        }
    }

    /// <summary>
    /// Initialize the command registry using reflection to find all command types
    /// </summary>
    private void InitializeCommandRegistry()
    {
        AddCommandsFromAssemblyOf<CommandAttribute>();
    }

    /// <summary>
    /// Get the appropriate command type for a given raw name and current protocol version
    /// (matches TypeScript commandFromRawName logic in atemCommandParser.ts:24-50)
    /// </summary>
    /// <param name="rawName">4-character command name</param>
    /// <returns>Best matching command type or null if not found/supported</returns>
    private Type? GetCommandTypeForVersion(string rawName)
    {
        // Find the highest version number that is lower or equal to the current version
        return _commandRegistry.GetValueOrDefault(rawName)?.Last(x => x.Key <= Version).Value;
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

        // Call static Deserialize method (matches TypeScript cmdConstructor.deserialize pattern)
        var deserializeMethod = commandType.GetMethod("Deserialize",
                                                      BindingFlags.Static | BindingFlags.Public);

        // TODO #66: Resolve logger and log here
        if (deserializeMethod == null)
        {
            throw new InvalidOperationException($"Command {commandType.Name} missing static Deserialize method");
        }

        var command = deserializeMethod.CreateDelegate<DeserializeCommand>()(data, Version);

        // Update parser version if this is a VersionCommand (matches TypeScript behavior)
        if (command is VersionCommand versionCmd)
        {
            Version = versionCmd.Version;
        }

        return command;
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
        return _commandRegistry.TryGetValue(rawName, out var commandTypes)
                   ? commandTypes.Values.AsReadOnly()
                   : new List<Type>().AsReadOnly();
    }

    /// <summary>
    /// Get all registered command names (for testing/debugging)
    /// </summary>
    /// <returns>Collection of all registered raw command names</returns>
    public IReadOnlyCollection<string> GetRegisteredCommands()
    {
        return _commandRegistry.Keys;
    }
}
