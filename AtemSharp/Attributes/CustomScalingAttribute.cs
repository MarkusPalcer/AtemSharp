using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.Attributes;

[AttributeUsage(AttributeTargets.Field)]
[SuppressMessage("ReSharper", "UnusedParameter.Local")]
public class CustomScalingAttribute : Attribute
{
    public CustomScalingAttribute(string customScalingMethod) {}
}
