using System.Reflection;
using AtemSharp.Commands;
using AtemSharp.Commands.DeviceProfile;
using AtemSharp.Commands.MixEffects;
using AtemSharp.Enums;

namespace AtemSharp.Net;

/// <summary>
/// Parses ATEM commands from received data
/// </summary>
internal class CommandParser
{
    private readonly Dictionary<string, List<Type>> _commands = new();
    private ProtocolVersion _version = ProtocolVersion.V7_2; // Default to minimum supported

    public ProtocolVersion Version
    {
        get => _version;
        set => _version = value;
    }

    public CommandParser()
    {
        RegisterCommands();
    }

    /// <summary>
    /// Parse commands from a received packet
    /// </summary>
    /// <param name="buffer">Packet data</param>
    /// <returns>Array of parsed commands</returns>
    public IDeserializedCommand[] ParseCommands(byte[] buffer)
    {
        var commands = new List<IDeserializedCommand>();
        var offset = 0;

        while (offset < buffer.Length)
        {
            if (offset + 8 > buffer.Length)
                break; // Not enough data for header

            // Read command header
            var length = ReadUInt16BE(buffer, offset);
            var commandName = System.Text.Encoding.ASCII.GetString(buffer, offset + 4, 4);

            if (length < 8 || offset + length > buffer.Length)
                break; // Invalid command length

            // Extract command payload
            var payload = new byte[length - 8];
            if (payload.Length > 0)
                Array.Copy(buffer, offset + 8, payload, 0, payload.Length);

            // Try to parse command
            var command = ParseCommand(commandName, payload);
            if (command != null)
                commands.Add(command);

            offset += length;
        }

        return commands.ToArray();
    }

    /// <summary>
    /// Parse a single command by name and payload
    /// </summary>
    /// <param name="commandName">4-character command name</param>
    /// <param name="payload">Command payload data</param>
    /// <returns>Parsed command or null if not recognized</returns>
    public IDeserializedCommand? ParseCommand(string commandName, byte[] payload)
    {
        var commandType = GetCommandTypeFromRawName(commandName);
        if (commandType == null)
            return null;

        try
        {
            // Use reflection to call the static Deserialize method
            var deserializeMethod = commandType.GetMethod("Deserialize", 
                BindingFlags.Public | BindingFlags.Static);
            
            if (deserializeMethod != null)
            {
                var result = deserializeMethod.Invoke(null, new object[] { payload });
                return result as IDeserializedCommand;
            }
        }
        catch (Exception)
        {
            // Failed to parse command, ignore
        }

        return null;
    }

    private Type? GetCommandTypeFromRawName(string name)
    {
        if (!_commands.TryGetValue(name, out var commandTypes))
            return null;

        if (commandTypes.Count == 0)
            return null;

        if (_version == ProtocolVersion.V7_2)
        {
            // Edge case for version command itself
            return commandTypes[0];
        }

        // Find the appropriate command version
        var baseline = commandTypes.FirstOrDefault(cmd => GetMinimumVersion(cmd) == null);
        var overrides = commandTypes
            .Where(cmd => GetMinimumVersion(cmd) != null && GetMinimumVersion(cmd) <= _version)
            .ToList();

        if (overrides.Count == 0)
            return baseline;

        // Find highest version in overrides
        Type? highestProtoCommand = overrides[0];
        foreach (var cmd in overrides)
        {
            var currentMin = GetMinimumVersion(highestProtoCommand);
            var cmdMin = GetMinimumVersion(cmd);
            
            if (currentMin != null && cmdMin != null && cmdMin > currentMin)
                highestProtoCommand = cmd;
        }

        return highestProtoCommand;
    }

    private static ProtocolVersion? GetMinimumVersion(Type commandType)
    {
        var property = commandType.GetProperty("MinimumVersion", 
            BindingFlags.Public | BindingFlags.Static);
        return property?.GetValue(null) as ProtocolVersion?;
    }

    private static string? GetRawName(Type commandType)
    {
        var property = commandType.GetProperty("RawName", 
            BindingFlags.Public | BindingFlags.Static);
        return property?.GetValue(null) as string;
    }

    private void RegisterCommands()
    {
        // Register all known command types
        RegisterCommand<VersionCommand>();
        RegisterCommand<InitCompleteCommand>();
        RegisterCommand<ProgramInputUpdateCommand>();
        RegisterCommand<PreviewInputUpdateCommand>();
        
        // TODO: Add more commands as they are implemented
    }

    private void RegisterCommand<T>() where T : class, IDeserializedCommand
    {
        var type = typeof(T);
        var rawName = GetRawName(type);
        
        if (!string.IsNullOrEmpty(rawName))
        {
            if (!_commands.ContainsKey(rawName))
                _commands[rawName] = new List<Type>();
            
            _commands[rawName].Add(type);
        }
    }

    private static ushort ReadUInt16BE(byte[] buffer, int offset)
    {
        return (ushort)((buffer[offset] << 8) | buffer[offset + 1]);
    }
}