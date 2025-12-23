using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.Attributes;

/// <summary>
/// Tells code generation that this field is deserialized from the received data.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
[SuppressMessage("ReSharper", "UnusedParameter.Local")]
[ExcludeFromCodeCoverage]
internal class DeserializedFieldAttribute : Attribute
{
    /// <param name="offset">The index of the field in the byte buffer</param>
    public DeserializedFieldAttribute(int offset)
    {
    }
}
