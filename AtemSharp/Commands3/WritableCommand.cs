using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AtemSharp.Enums;

namespace AtemSharp.Commands3;

/// <summary>
/// Property metadata for reflection-based command handling
/// </summary>
public record PropertyMetadata(byte MaskFlag, int Order, PropertyInfo PropertyInfo)
{
    public Func<object, object?> Getter { get; } = obj => PropertyInfo.GetValue(obj);
    public Action<object, object?> Setter { get; } = (obj, val) => PropertyInfo.SetValue(obj, val);
}

/// <summary>
/// Base class for writable commands using reflection-based property mapping
/// </summary>
/// <typeparam name="TSelf">The concrete command type</typeparam>
public abstract class WritableCommand<TSelf> : BasicWritableCommand
    where TSelf : WritableCommand<TSelf>
{
    private static readonly Dictionary<Type, PropertyMetadata[]> _propertyMapCache = new();
    
    /// <summary>
    /// Property change flags
    /// </summary>
    public byte Flag { get; protected set; }

    /// <summary>
    /// Get the property metadata for this command type, using reflection and caching
    /// </summary>
    protected PropertyMetadata[] PropertyMap => GetPropertyMap(typeof(TSelf));

    /// <summary>
    /// Update the values of some properties with this command
    /// </summary>
    /// <param name="newValue">Source object with new property values</param>
    /// <returns>True if any properties were changed</returns>
    public bool UpdateProps(TSelf newValue)
    {
        return UpdatePropsInternal(newValue);
    }

    protected bool UpdatePropsInternal(TSelf newValue)
    {
        bool hasChanges = false;
        
        foreach (var property in PropertyMap)
        {
            var currentValue = property.Getter(this);
            var newPropertyValue = property.Getter(newValue);
            
            // Check if values are different (handling nulls properly)
            if (!Equals(currentValue, newPropertyValue))
            {
                Flag |= property.MaskFlag;
                property.Setter(this, newPropertyValue);
                hasChanges = true;
            }
        }
        
        return hasChanges;
    }

    /// <summary>
    /// Get property metadata for a command type, using reflection and caching
    /// </summary>
    /// <param name="commandType">The command type to analyze</param>
    /// <returns>Array of property metadata ordered by Order attribute</returns>
    private static PropertyMetadata[] GetPropertyMap(Type commandType)
    {
        if (_propertyMapCache.TryGetValue(commandType, out var cached))
        {
            return cached;
        }

        var properties = commandType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(prop => prop.GetCustomAttribute<CommandPropertyAttribute>() != null)
            .Select(prop =>
            {
                var commandAttr = prop.GetCustomAttribute<CommandPropertyAttribute>()!;
                return new PropertyMetadata(commandAttr.MaskFlag, commandAttr.Order, prop);
            })
            .OrderBy(pm => pm.Order)
            .ThenBy(pm => pm.MaskFlag) // Secondary sort by mask flag if orders are equal
            .ToArray();

        _propertyMapCache[commandType] = properties;
        return properties;
    }

    /// <summary>
    /// Clear the property map cache (useful for testing or dynamic scenarios)
    /// </summary>
    public static void ClearPropertyMapCache()
    {
        _propertyMapCache.Clear();
    }
}

/// <summary>
/// Base command interface for serializable commands
/// </summary>
public abstract class BasicWritableCommand : ICommand
{
    /// <summary>
    /// Serialize command to binary stream
    /// </summary>
    /// <param name="version">Protocol version</param>
    /// <returns>Serialized command data</returns>
    public abstract Stream Serialize(ProtocolVersion version);
}