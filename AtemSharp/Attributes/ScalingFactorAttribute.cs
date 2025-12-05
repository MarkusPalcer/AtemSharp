using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.Attributes;

/// <summary>
/// Tells the code generator to scale the field by the given value when (de-)serializing
/// </summary>
/// <remarks>
/// <ul>
/// <li>The value is <i>divided</i> by the scaling factor upon deserialization and <i>multiplied</i> upon serialization</li>
/// <li>This serialization is applied <i>after</i> the custom scaling function upon deserialization and <i>before</i> it upon serialization</li>
/// </ul>
/// </remarks>
[AttributeUsage(AttributeTargets.Field)]
[SuppressMessage("ReSharper", "UnusedParameter.Local")]
[ExcludeFromCodeCoverage]
public class ScalingFactorAttribute : Attribute
{
    public ScalingFactorAttribute(double factor)
    {
    }
}
