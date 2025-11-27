using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.Attributes;

/// <summary>
/// Marks that a field won't get a generated property. This is for ID fields which are set in the constructor and never changed.
/// </summary>
/// <remarks>
/// Mutually exclusive with <see cref="CustomSerializationAttribute"/>
/// </remarks>
[AttributeUsage(AttributeTargets.Field)]
[ExcludeFromCodeCoverage]
public class NoPropertyAttribute : Attribute
{

}
