using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.Attributes;

/// <summary>
/// Specifies which method validates values of this field when the generated property is being set
/// </summary>
/// <remarks>
/// This has no effect if the <see cref="NoPropertyAttribute"/> is also present
/// </remarks>
[AttributeUsage(AttributeTargets.Field)]
[SuppressMessage("ReSharper", "UnusedParameter.Local")]
public class ValidationMethodAttribute : Attribute
{
    public ValidationMethodAttribute(string methodName) {}
}
