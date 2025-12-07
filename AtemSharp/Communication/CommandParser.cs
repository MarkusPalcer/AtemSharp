using System.Diagnostics;
using System.Reflection;
using AtemSharp.Commands;
using AtemSharp.State.Info;

namespace AtemSharp.Communication;

/// <summary>
/// Parses ATEM commands from raw packet data using reflection-based command registry
/// </summary>
internal class CommandParser : ICommandParser
{
    internal readonly Dictionary<string, SortedList<ProtocolVersion,Type>> CommandRegistry = new();

    /// <summary>
    /// Current protocol version for parsing commands
    /// </summary>
    public ProtocolVersion Version { get; set; } = ProtocolVersion.Unknown;

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
            if (!CommandRegistry.ContainsKey(attr.RawName))
            {
                CommandRegistry[attr.RawName] = [];
            }

            CommandRegistry[attr.RawName].Add(attr.MinimumVersion ?? ProtocolVersion.Unknown, type);
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
    internal Type? GetCommandTypeForVersion(string rawName)
    {
        // Find the highest version number that is lower or equal to the current version
        return CommandRegistry.GetValueOrDefault(rawName)?.Last(x => x.Key <= Version).Value;
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

        Debug.WriteLineIf(deserializeMethod == null, $"Command {commandType.Name} missing static Deserialize method");

        var command = deserializeMethod?.CreateDelegate<DeserializeCommand>()(data, Version);

        return command;
    }
}
