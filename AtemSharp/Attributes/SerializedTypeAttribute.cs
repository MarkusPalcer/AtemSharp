using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.Attributes;

[AttributeUsage(AttributeTargets.Field)]
[SuppressMessage("ReSharper", "UnusedParameter.Local")]
public class SerializedTypeAttribute : Attribute
{
    public SerializedTypeAttribute(Type type) {}
}
