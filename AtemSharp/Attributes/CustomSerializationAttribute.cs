namespace AtemSharp.Helpers;

/// <summary>
/// Marks that a field won't be included in the generated serialization logic but will have a generated property
/// </summary>
/// <remarks>
/// Mutually exclusive with <see cref="NoPropertyAttribute"/>, as it would mean no code is generated
/// </remarks>
[AttributeUsage(AttributeTargets.Field)]
public class CustomSerializationAttribute : Attribute
{
}
