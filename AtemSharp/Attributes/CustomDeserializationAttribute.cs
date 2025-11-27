using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.Attributes;

/// <summary>
/// Tells code generation to create a Property for this field but no deserialization code <br />
/// The deserialization code should be in <code>DeserializeInternal</code>
/// </summary>
/// <remarks>
/// Mutually exclusive with <see cref="NoPropertyAttribute"/>
/// </remarks>
[AttributeUsage(AttributeTargets.Field)]
[ExcludeFromCodeCoverage]
public class CustomDeserializationAttribute : Attribute
{
}
