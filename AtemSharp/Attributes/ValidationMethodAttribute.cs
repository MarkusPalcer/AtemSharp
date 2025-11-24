using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.Attributes;

[AttributeUsage(AttributeTargets.Field)]
[SuppressMessage("ReSharper", "UnusedParameter.Local")]
public class ValidationMethodAttribute : Attribute
{
    public ValidationMethodAttribute(string methodName) {}
}
