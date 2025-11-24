using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.Attributes;

[AttributeUsage(AttributeTargets.Class)]
[SuppressMessage("ReSharper", "UnusedParameter.Local")]
public class BufferSizeAttribute : Attribute
{
    public BufferSizeAttribute(int size)
    {
    }
}
