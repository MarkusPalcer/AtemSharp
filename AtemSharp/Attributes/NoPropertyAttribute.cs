namespace AtemSharp.Attributes;

/// <summary>
/// Marks that a field won't get a generated property. This is for ID fields which are set in the constructor and never changed.
/// </summary>
/// <remarks>
/// Mutually exclusive with <see cref="CustomSerializationAttribute"/>, as it would mean no code is generated
/// </remarks>
[AttributeUsage(AttributeTargets.Field)]
public class NoPropertyAttribute : Attribute
{

}
