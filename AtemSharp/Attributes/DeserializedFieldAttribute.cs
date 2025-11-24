using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.Attributes;

[AttributeUsage(AttributeTargets.Field)]
[SuppressMessage("ReSharper", "UnusedParameter.Local")]
public class DeserializedFieldAttribute : Attribute
{
    public DeserializedFieldAttribute(int offset)
    {
    }
}
