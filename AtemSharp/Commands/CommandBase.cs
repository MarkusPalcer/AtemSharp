using System.Runtime.Serialization;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands;

/// <summary>
/// Interface for commands that can be deserialized from received data
/// </summary>
public interface IDeserializedCommand
{
    /// <summary>
    /// Command properties
    /// </summary>
    object Properties { get; }
    
    /// <summary>
    /// Apply this command to the ATEM state
    /// </summary>
    /// <param name="state">ATEM state to modify</param>
    /// <returns>List of state paths that were changed</returns>
    string[] ApplyToState(AtemState state);
}

/// <summary>
/// Base type for a receivable command
/// </summary>
/// <typeparam name="T">Properties type</typeparam>
public abstract class DeserializedCommand<T> : IDeserializedCommand
{
    /// <summary>
    /// Raw command name
    /// </summary>
    public static string? RawName { get; protected set; }
    
    /// <summary>
    /// Minimum protocol version required
    /// </summary>
    public static ProtocolVersion? MinimumVersion { get; protected set; }
    
    /// <summary>
    /// Command properties
    /// </summary>
    public T Properties { get; }
    
    /// <summary>
    /// Command properties (untyped)
    /// </summary>
    object IDeserializedCommand.Properties => Properties!;

    protected DeserializedCommand(T properties)
    {
        Properties = properties;
    }

    /// <summary>
    /// Apply this command to the ATEM state
    /// </summary>
    /// <param name="state">ATEM state to modify</param>
    /// <returns>List of state paths that were changed</returns>
    public abstract string[] ApplyToState(AtemState state);
}

/// <summary>
/// Interface for commands that can be serialized to send to ATEM
/// </summary>
public interface ISerializableCommand
{
    /// <summary>
    /// Serialize command to binary data
    /// </summary>
    /// <param name="version">Protocol version</param>
    /// <returns>Serialized command data</returns>
    byte[] Serialize(ProtocolVersion version);
}

/// <summary>
/// Base command type for a simple writable command, which has a few values which must all be sent
/// </summary>
/// <typeparam name="T">Properties type</typeparam>
public abstract class BasicWritableCommand<T> : ISerializableCommand
{
    /// <summary>
    /// Raw command name
    /// </summary>
    public static string? RawName { get; protected set; }
    
    /// <summary>
    /// Minimum protocol version required
    /// </summary>
    public static ProtocolVersion? MinimumVersion { get; protected set; }

    protected T _properties;

    /// <summary>
    /// Command properties
    /// </summary>
    public T Properties => _properties;

    protected BasicWritableCommand(T properties)
    {
        _properties = properties;
    }

    /// <summary>
    /// Serialize command to binary data
    /// </summary>
    /// <param name="version">Protocol version</param>
    /// <returns>Serialized command data</returns>
    public abstract byte[] Serialize(ProtocolVersion version);
}

/// <summary>
/// Base command type for a full writable command, which uses flags to indicate the changed properties
/// </summary>
/// <typeparam name="T">Properties type</typeparam>
public abstract class WritableCommand<T> : BasicWritableCommand<Dictionary<string, object?>>
{
    /// <summary>
    /// Mask flags for properties
    /// </summary>
    public static Dictionary<string, int>? MaskFlags { get; protected set; }

    /// <summary>
    /// Property change flags
    /// </summary>
    public int Flag { get; protected set; }

    protected WritableCommand() : base(new Dictionary<string, object?>())
    {
        Flag = 0;
    }

    /// <summary>
    /// Update the values of some properties with this command
    /// </summary>
    /// <param name="newProps">Properties to update</param>
    /// <returns>True if any properties were changed</returns>
    public bool UpdateProps(Dictionary<string, object?> newProps)
    {
        return UpdatePropsInternal(newProps);
    }

    protected bool UpdatePropsInternal(Dictionary<string, object?> newProps)
    {
        var maskFlags = MaskFlags;
        bool hasChanges = false;
        
        if (maskFlags != null)
        {
            foreach (var kvp in newProps)
            {
                if (maskFlags.TryGetValue(kvp.Key, out int flagValue) && kvp.Value != null)
                {
                    Flag |= flagValue;
                    _properties[kvp.Key] = kvp.Value;
                    hasChanges = true;
                }
            }
        }
        
        return hasChanges;
    }
}

/// <summary>
/// Base command type for a command which gets sent in both directions
/// </summary>
/// <typeparam name="T">Properties type</typeparam>
public abstract class SymmetricalCommand<T> : DeserializedCommand<T>, ISerializableCommand
{
    protected SymmetricalCommand(T properties) : base(properties)
    {
    }

    /// <summary>
    /// Serialize command to binary data
    /// </summary>
    /// <param name="version">Protocol version</param>
    /// <returns>Serialized command data</returns>
    public abstract byte[] Serialize(ProtocolVersion version);
}