using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.Attributes;

/// <summary>
/// Tells code generation the size of the serialization buffer for this command
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
[SuppressMessage("ReSharper", "UnusedParameter.Local")]
[ExcludeFromCodeCoverage]
internal class BufferSizeAttribute : Attribute
{
    public BufferSizeAttribute(int size)
    {
    }
}
