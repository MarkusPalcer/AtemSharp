using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.Attributes;

[AttributeUsage(AttributeTargets.Field)]
[SuppressMessage("ReSharper", "UnusedParameter.Local")]
public class SerializedFieldAttribute : Attribute
{
    public SerializedFieldAttribute(int offset, byte flag)
    {
    }

    public SerializedFieldAttribute(int offset)
    {
    }
}
