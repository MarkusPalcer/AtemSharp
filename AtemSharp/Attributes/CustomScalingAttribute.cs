using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.Attributes;

/// <summary>
/// Tells code generation to invoke a method to scale a value when (de-)serializing this field
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
[SuppressMessage("ReSharper", "UnusedParameter.Local")]
[ExcludeFromCodeCoverage]
internal class CustomScalingAttribute : Attribute
{
    public CustomScalingAttribute(string customScalingMethod) {}
}
