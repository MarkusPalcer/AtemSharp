using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.Attributes;

[AttributeUsage(AttributeTargets.Field)]
[SuppressMessage("ReSharper", "UnusedParameter.Local")]
public class ScalingFactorAttribute : Attribute
{
    public ScalingFactorAttribute(double factor)
    {
    }
}
