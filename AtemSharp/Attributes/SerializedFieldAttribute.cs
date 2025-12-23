using System.Diagnostics.CodeAnalysis;
using AtemSharp.Commands;

namespace AtemSharp.Attributes;

/// <summary>
/// Tells code generation that this field is serialized from the received data.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
[SuppressMessage("ReSharper", "UnusedParameter.Local")]
[ExcludeFromCodeCoverage]
internal class SerializedFieldAttribute : Attribute
{
    /// <param name="offset">The index of the field in the byte buffer</param>
    /// <param name="flag">
    /// The index of the bit in the <see cref="SerializedCommand.Flag"/>-property to set when the
    /// value of this field is changed
    /// </param>
    public SerializedFieldAttribute(int offset, byte flag)
    {
    }

    /// <param name="offset">The index of the field in the byte buffer</param>
    public SerializedFieldAttribute(int offset)
    {
    }
}
