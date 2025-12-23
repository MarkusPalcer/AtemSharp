using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.Attributes;

/// <summary>
/// Tells code generation to create a Property for this field but no serialization code <br />
/// The serialization code should be in <code>SerializeInternal</code>
/// </summary>
/// <remarks>
/// Mutually exclusive with <see cref="NoPropertyAttribute"/>
/// </remarks>
[AttributeUsage(AttributeTargets.Field)]
[SuppressMessage("ReSharper", "UnusedParameter.Local")]
[ExcludeFromCodeCoverage]
internal class CustomSerializationAttribute : Attribute
{
    /// <summary>
    /// Marks the field to be serialized with a custom logic, but does not change the Flag property when the value of
    /// the generated property is changed
    /// </summary>
    public CustomSerializationAttribute()
    {
    }

    /// <summary>
    /// Marks the field to be serialized with a custom logic and sets the specified bit of the Flag property when the
    /// value of the generated property is changed
    /// </summary>
    public CustomSerializationAttribute(byte flag)
    {
    }
}
