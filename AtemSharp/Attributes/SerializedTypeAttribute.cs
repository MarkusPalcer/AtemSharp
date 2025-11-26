using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.Attributes;

/// <summary>
/// Tells the code generation which type is written to the byte buffer; overrides the type of the field
/// </summary>
/// <remarks>
/// This is mainly used for <code>double</code> fields, because we write them scaled and as integral values, so we use
/// this in conjunction with <see cref="ScalingFactorAttribute"/>
/// </remarks>
[AttributeUsage(AttributeTargets.Field)]
[SuppressMessage("ReSharper", "UnusedParameter.Local")]
public class SerializedTypeAttribute : Attribute
{
    public SerializedTypeAttribute(Type type) {}
}
