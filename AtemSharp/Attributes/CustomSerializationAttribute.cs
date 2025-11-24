using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.Attributes;

/// <summary>
/// Marks that a field won't be included in the generated serialization logic but will have a generated property
/// </summary>
/// <remarks>
/// Mutually exclusive with <see cref="NoPropertyAttribute"/>, as it would mean no code is generated
/// </remarks>
[AttributeUsage(AttributeTargets.Field)]
[SuppressMessage("ReSharper", "UnusedParameter.Local")]
public class CustomSerializationAttribute : Attribute
{
    public CustomSerializationAttribute() {}

    public CustomSerializationAttribute(byte flag) {}

}
