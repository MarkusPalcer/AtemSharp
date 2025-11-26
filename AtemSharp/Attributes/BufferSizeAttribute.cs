using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.Attributes;

/// <summary>
/// Tells code generation the size of the serialization buffer for this command
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
[SuppressMessage("ReSharper", "UnusedParameter.Local")]
public class BufferSizeAttribute : Attribute
{
    public BufferSizeAttribute(int size)
    {
    }
}
